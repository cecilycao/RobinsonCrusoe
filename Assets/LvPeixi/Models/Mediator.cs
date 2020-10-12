using System.Collections;
using System.Collections.Generic;
using UnityEngine.Assertions;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// Mediator负责多个系统之间协调、判定
/// </summary>
public class Mediator : MonoBehaviour,IMediator
{
    #region//-----private variables-----
    static IMediator _instance;
    IInteractableNPC theCurrentInteractNPC;
    /// <summary>
    /// 调用玩家的互动指令
    /// </summary>
    IPlayerInteractPresenter playerInteract;
    /// <summary>
    /// 过滤掉多个密集请求
    /// </summary>
    protected bool IsAtInteractState = false;
    #endregion

    #region//-----initialize-----
    public static IMediator Sigton
    {
        get => _instance;
        set
        {
            if (_instance == null)
            {
                _instance = value;
            }
        }
    }
   
    private void Awake()
    {
        Sigton = this;
        Assert.IsNull(playerInteract, "Mediator.playerInteract is null");
    }
    #endregion

    #region//IMediator implement
    public void StartDialog(IInteractableNPC npc)
    {
        
    }
    public void EndDialog()
    {
        if (theCurrentInteractNPC!= null)
        {
            theCurrentInteractNPC.EndContact();
            theCurrentInteractNPC = null;
        }
        AssertExtension.NotNullRun(GameEvents.Sigton.onDialogEnd, () =>
        {
            GameEvents.Sigton.onDialogEnd.Invoke();
        });
        AssertExtension.NotNullRun(GameEvents.Sigton.onInteractEnd, () =>
        {
            GameEvents.Sigton.onInteractEnd.Invoke();
        });
    }
    public void StartResourceCollect(IInteractableResourceCollector collector)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键收集资源");

            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    collector.StartInteract();

                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    //start fishing game
                    GUIEvents.Singleton.PlayerStartFishing.OnNext(true);
                    //set player interact state
                    playerInteract.PlayerStartInteraction(PlayerInteractionType.Collect);
                    //end collect resource after 1 sec
                    Observable.Timer(TimeSpan.FromSeconds(1))
                    .First()
                    .Subscribe(y =>
                    {
                        playerInteract.PlayerEndInteraction();
                        GameEvents.Sigton.onInteractEnd();
                    });
                });

            GUIEvents.Singleton.PlayerEndFishing
                .First()
                .Subscribe(x =>
                {  
                    if (x)
                    {
                        AssertExtension.NotNullRun(GameEvents.Sigton.onResourceCollected, () =>
                        {
                            GameEvents.Sigton.onResourceCollected.Invoke(collector.ResourceType, collector.ResourceAccount);
                        });
                    }
                    else
                    {
                        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("什么也没有捞到");
                        Observable.Timer(TimeSpan.FromSeconds(1))
                            .First()
                            .Subscribe(y =>
                            {
                                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                            });
                    }
                    collector.EndInteract((object)x);
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    public void StartAddIsland(IslandBuilder builder)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;

            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            var playerBuildingMaterial = inventory.BuildingMaterial.Value;
            if (playerBuildingMaterial < builder.MaterialCost)
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("building material is not enough");
                return;
            }
            
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键创建岛屿"); 

            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("正在建造新浮岛");
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(2);
                });

            IDisposable theEventCompleteProgress = null;
            theEventCompleteProgress = InputSystem.Singleton.OnInteractBtnPressed
                .Delay(TimeSpan.FromSeconds(2))
                .First()
                .Subscribe(x =>
                {
                    builder.OnIslandBuildEnd();
                    inventory.BuildingMaterial.Value -= builder.MaterialCost;
                    GameEvents.Sigton.onInteractEnd();
                });
                

            InputSystem.Singleton.OnInteractBtnReleased
                .First()
                .Subscribe(x =>
                {
                    GameEvents.Sigton.onInteractEnd();
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                    theEventCompleteProgress.Dispose();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    public void StartRestoreIsland(IInteractableIsland island)
    {
        
    }
    public void StartProcessFood(IFoodProcess foodProcess)
    {
        
    }
    public void EndInteract()
    {
        GameEvents.Sigton.onInteractEnd();
    }
    /// <summary>
    /// 和NPC对话
    /// </summary>
    /// <param name="npc"></param>
    public void StartInteraction(IInteractableNPC npc)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;
            IDisposable waitForKeyboardInteractSingle = null;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键对话");

            waitForKeyboardInteractSingle = Observable.EveryUpdate()
                .Where(x => Input.GetKeyDown(KeyCode.E))
                .Subscribe(x =>
                {
                    theCurrentInteractNPC = npc;
                    DialogManager.Singelton.StartDialog(npc.NPCName);
                    npc.StartInteract();
                    playerInteract.PlayerStartInteraction(PlayerInteractionType.Dialog);
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    waitForKeyboardInteractSingle.Dispose();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                waitForKeyboardInteractSingle.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    /// <summary>
    /// 收集资源
    /// </summary>
    /// <param name="collector"></param>
    public void StartInteraction(IInteractableResourceCollector collector)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键收集资源");

            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    collector.StartInteract();

                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    //start fishing game
                    GUIEvents.Singleton.PlayerStartFishing.OnNext(true);
                    //set player interact state
                    playerInteract.PlayerStartInteraction(PlayerInteractionType.Collect);
                    //end collect resource after 1 sec
                    Observable.Timer(TimeSpan.FromSeconds(1))
                    .First()
                    .Subscribe(y =>
                    {
                        playerInteract.PlayerEndInteraction();
                        GameEvents.Sigton.onInteractEnd();
                    });
                });

            GUIEvents.Singleton.PlayerEndFishing
                .First()
                .Subscribe(x =>
                {
                    if (x)
                    {
                        AssertExtension.NotNullRun(GameEvents.Sigton.onResourceCollected, () =>
                        {
                            GameEvents.Sigton.onResourceCollected.Invoke(collector.ResourceType, collector.ResourceAccount);
                        });
                    }
                    else
                    {
                        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("什么也没有捞到");
                        Observable.Timer(TimeSpan.FromSeconds(1))
                            .First()
                            .Subscribe(y =>
                            {
                                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                            });
                    }
                    collector.EndInteract((object)x);
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    /// <summary>
    /// 新建浮岛
    /// </summary>
    /// <param name="builder"></param>
    public void StartInteraction(IIslandBuilder builder)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;

            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            var playerBuildingMaterial = inventory.BuildingMaterial.Value;
            if (playerBuildingMaterial < builder.MaterialCost)
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("building material is not enough");
                return;
            }

            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键创建岛屿");

            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("正在建造新浮岛");
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(2);
                    builder.StartInteract();
                });

            IDisposable theEventCompleteProgress = null;
            theEventCompleteProgress = InputSystem.Singleton.OnInteractBtnPressed
                .Delay(TimeSpan.FromSeconds(2))
                .First()
                .Subscribe(x =>
                {
                    inventory.BuildingMaterial.Value -= builder.MaterialCost;
                    builder.EndInteract(true);
                    GameEvents.Sigton.onInteractEnd();
                });


            InputSystem.Singleton.OnInteractBtnReleased
                .First()
                .Subscribe(x =>
                {
                    GameEvents.Sigton.onInteractEnd();
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                    theEventCompleteProgress.Dispose();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                builder.EndContact();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    /// <summary>
    /// 加工食物
    /// </summary>
    /// <param name="foodProcess"></param>
    public void StartInteraction(IFoodProcess foodProcess)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;
            var attr = FindObjectOfType<PlayerAttributePresenter>();
            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            //check player has enough food material?
            if (inventory.FoodMaterial.Value < foodProcess.Cost)
            {
                IsAtInteractState = false;
                return;
            }
  
            if (foodProcess.HasFood)
            {
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键吃食物");
                InputSystem.Singleton.OnInteractBtnPressed
                    .First()
                    .Subscribe(x =>
                    {
                        attr.Hunger.Value -= foodProcess.HungerRestore;
                        foodProcess.EndInteract(false);
                        GameEvents.Sigton.onInteractEnd();
                    });
            }
            else
            {
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按住E键生产食物");
                //listen the interact key pressed event
                InputSystem.Singleton.OnInteractBtnPressed
                    .First()
                    .Subscribe(x =>
                    {
                        foodProcess.StartInteract();
                        GUIEvents.Singleton.InteractionProgressBar.OnNext(2);
                        GameEvents.Sigton.onInteractEnd();
                    });

                IDisposable theEventCompleteProgress = null;
                theEventCompleteProgress = InputSystem.Singleton.OnInteractBtnPressed
                    .Delay(TimeSpan.FromSeconds(2))
                    .First()
                    .Subscribe(x =>
                    {
                        inventory.FoodMaterial.Value -= foodProcess.Cost;
                        foodProcess.EndInteract(true);
                        GameEvents.Sigton.onInteractEnd();
                    });

                InputSystem.Singleton.OnInteractBtnReleased
                    .First()
                    .Subscribe(x =>
                    {
                        GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                        GameEvents.Sigton.onInteractEnd();
                        //foodProcess.EndInteract(true);
                        theEventCompleteProgress.Dispose();
                    });
            }
            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    /// <summary>
    /// 维修岛屿
    /// </summary>
    /// <param name="island"></param>
    public void StartInteraction(IInteractableIsland island)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;

            //check player have enough resource?
            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            var playerBuildingMaterial = inventory.BuildingMaterial.Value;
            if (playerBuildingMaterial < island.MaterialCost)
            {
                IsAtInteractState = false;
                return;
            }
            //listen the interact key pressed event
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键修复浮岛");
           
            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    island.StartInteract();
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(2);
                    island.EndInteract(true);
                    GameEvents.Sigton.onInteractEnd();
                });

            IDisposable theEventCompleteProgress = null;
            theEventCompleteProgress = InputSystem.Singleton.OnInteractBtnPressed
                .Delay(TimeSpan.FromSeconds(2))
                .First()
                .Subscribe(x =>
                {
                    inventory.BuildingMaterial.Value -= island.MaterialCost;
                    GameEvents.Sigton.onInteractEnd();
                    island.EndInteract(true);
                });
            
            InputSystem.Singleton.OnInteractBtnReleased
                .First()
                .Subscribe(x =>
                {
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    public IPlayerInteractPresenter PlayerInteract
    {
        set
        {
            if (playerInteract == null)
            {
                playerInteract = value;
            }
        }
    }
    #endregion
}
