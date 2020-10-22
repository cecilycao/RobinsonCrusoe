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
    /// 调用玩家数值指令
    /// </summary>
    IPlayerAttribute playerAttribute;
    /// <summary>
    /// 过滤掉多个密集请求
    /// </summary>
    protected bool IsAtInteractState = false;
    private Dictionary<string, float> interactConfig;
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
    private void Start()
    {
        interactConfig = GameConfig.Singleton.InteractionConfig;
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
            collector.ShowIcon();

            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    collector.StartInteract();

                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    collector.HideIcon();
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
                collector.HideIcon();
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
        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
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
            npc.ShowIcon();

            waitForKeyboardInteractSingle = Observable.EveryUpdate()
                .Where(x => Input.GetKeyDown(KeyCode.E))
                .Subscribe(x =>
                {
                    
                    theCurrentInteractNPC = npc;
                    DialogManager.Singelton.StartDialog(npc.NPCName);
                    npc.StartInteract();
                    playerInteract.PlayerStartInteraction(PlayerInteractionType.Dialog);
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    npc.HideIcon();
                    waitForKeyboardInteractSingle.Dispose();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                waitForKeyboardInteractSingle.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                npc.HideIcon();
            };
        }
    }
    /// <summary>
    /// 主动收集资源，发起钓鱼小游戏
    /// </summary>
    /// <param name="collector"></param>
    public void StartInteraction(IInteractableResourceCollector collector)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键收集资源");
            collector.ShowIcon();
        
            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    collector.StartInteract();

                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    collector.HideIcon();
                    //start fishing game
                    GUIEvents.Singleton.PlayerStartFishing.OnNext(true);
                    //set player interact state
                    playerInteract.PlayerStartInteraction(PlayerInteractionType.Collect);
                    //change player attribute
                    int _fatigueChange = (int)interactConfig["positiveCollectFatigueIncrease"];
                    playerAttribute.Fatigue.Value += _fatigueChange;

                    var _hungerChange = (int)interactConfig["interact_positiveCollect_hungerDecrea_default"];
                    playerAttribute.Hunger.Value += _hungerChange;

                    SendMesOutSideOnInteractBtnPressed();

                    //end collect resource after 1 sec
                    //Observable.Timer(TimeSpan.FromSeconds(1))
                    //.First()
                    //.Subscribe(y =>
                    //{
                    //    playerInteract.PlayerEndInteraction();
                    //    GameEvents.Sigton.onInteractEnd();
                    //});
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
                playerInteract.PlayerEndInteraction();
                collector.HideIcon();
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
            builder.ShowIcon();

            float buildIslandCostTime = interactConfig["addIslandTimeCost"];

            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    SendMesOutSideOnInteractBtnPressed();
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("正在建造新浮岛");                 
                    builder.HideIcon();
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(buildIslandCostTime);
                    builder.StartInteract();
                });

            IDisposable theEventCompleteProgress = null;
            theEventCompleteProgress = InputSystem.Singleton.OnInteractBtnPressed
                .Delay(TimeSpan.FromSeconds(buildIslandCostTime))
                .First()
                .Subscribe(x =>
                {
                    //-----set player attribute
                    int fatigueIncrease = (int)interactConfig["addIslandFatigueIncrease"];
                    playerAttribute.Fatigue.Value += fatigueIncrease;

                    var _hungerChange = (int)interactConfig["interact_addIsland_hungerDecrea_default"];
                    playerAttribute.Hunger.Value += _hungerChange;

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
                    AudioManager.Singleton.PlayAudio("Interact_build_restoreIsland_processFoodComplete");
                    theEventCompleteProgress.Dispose();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                builder.EndContact();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                builder.HideIcon();
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
                foodProcess.ShowIcon();
                InputSystem.Singleton.OnInteractBtnPressed
                    .First()
                    .Subscribe(x =>
                    {
                        foodProcess.HideIcon();
                        int _hungerRes = (int)interactConfig["eatFoodHungerIncrease"];
                        attr.Hunger.Value -= _hungerRes;
                        foodProcess.EndInteract(false);
                        GameEvents.Sigton.onInteractEnd();
                    });
            }
            else
            {
                float processFoodCostTime = interactConfig["processFoodTimeCost"];
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按住E键生产食物");
                foodProcess.ShowIcon();
                //listen the interact key pressed event
                InputSystem.Singleton.OnInteractBtnPressed
                    .First()
                    .Subscribe(x =>
                    {
                        SendMesOutSideOnInteractBtnPressed();
                        foodProcess.HideIcon();
                        foodProcess.StartInteract();
                        GUIEvents.Singleton.InteractionProgressBar.OnNext(processFoodCostTime);
                        AudioManager.Singleton.PlayAudio("Interact_processingFood");
                        GameEvents.Sigton.onInteractEnd();
                    });

                IDisposable theEventCompleteProgress = null;
                theEventCompleteProgress = InputSystem.Singleton.OnInteractBtnPressed
                    .Delay(TimeSpan.FromSeconds(processFoodCostTime))
                    .First()
                    .Subscribe(x =>
                    {
                        inventory.FoodMaterial.Value -= foodProcess.Cost;
                        int fatigueIncrease = (int)interactConfig["processFoodFatigeIncrease"];
                        playerAttribute.Fatigue.Value += fatigueIncrease;
                        var _hungerChanged = (int)interactConfig["interact_processFood_hungerDecrea_default"];
                        playerAttribute.Hunger.Value -= _hungerChanged;

                        AudioManager.Singleton.PlayAudio("Interact_build_restoreIsland_processFoodComplete");
                        foodProcess.EndInteract(true);
                        GameEvents.Sigton.onInteractEnd();
                    });

                InputSystem.Singleton.OnInteractBtnReleased
                    .First()
                    .Subscribe(x =>
                    {
                        GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                        GameEvents.Sigton.onInteractEnd();
                        AudioManager.Singleton.PauseAudio("Interact_processingFood");
                        //foodProcess.EndInteract(true);
                        theEventCompleteProgress.Dispose();
                    });
            }
            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                foodProcess.HideIcon();
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
            //-----get config----
            float restoreIslandCostTime = interactConfig["restoreIslandTimeCost"];

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
            island.ShowIcon();
           
            InputSystem.Singleton.OnInteractBtnPressed
                .First()
                .Subscribe(x =>
                {
                    SendMesOutSideOnInteractBtnPressed();
                    island.HideIcon();
                    AudioManager.Singleton.PlayAudio("Interact_islandRestoring");
                    island.StartInteract();
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(restoreIslandCostTime);
                    island.EndInteract(true);
                    GameEvents.Sigton.onInteractEnd();
                });

            IDisposable theEventCompleteProgress = null;
            theEventCompleteProgress = InputSystem.Singleton.OnInteractBtnPressed
                .Delay(TimeSpan.FromSeconds(restoreIslandCostTime))
                .First()
                .Subscribe(x =>
                {
                    AudioManager.Singleton.PauseAudio("Interact_islandRestoring");
                    int fatigueIncress = (int)interactConfig["restoreIslandFatigueIncrease"];
                    playerAttribute.Fatigue.Value += fatigueIncress;
                    var _hungerChange = (int)interactConfig["interact_restoreIsland_hungerDecrea_default"];
                    playerAttribute.Hunger.Value += _hungerChange;

                    inventory.BuildingMaterial.Value -= island.MaterialCost;
                    AudioManager.Singleton.PlayAudio("Interact_build_restoreIsland_processFoodComplete");
                    GameEvents.Sigton.onInteractEnd();
                    island.EndInteract(true);
                });

            IDisposable btnReleasedEvent = null;
            btnReleasedEvent = InputSystem.Singleton.OnInteractBtnReleased
                .First()
                .Subscribe(x =>
                {
                    AudioManager.Singleton.PauseAudio("Interact_islandRestoring");
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                btnReleasedEvent.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                island.HideIcon();
            };
        }
    }

    public void OpenDiary(Diary diary)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;
            IDisposable waitForKeyboardInteractSingle = null;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键对话");
            diary.ShowIcon();

            waitForKeyboardInteractSingle = Observable.EveryUpdate()
                .Where(x => Input.GetKeyDown(KeyCode.E))
                .Subscribe(x =>
                {
                    diary.OnDiaryOpen();
                    //playerInteract.PlayerStartInteraction(PlayerInteractionType.Dialog);
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    diary.HideIcon();
                    waitForKeyboardInteractSingle.Dispose();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                diary.OnDiaryClose();
                IsAtInteractState = false;
                waitForKeyboardInteractSingle.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                diary.HideIcon();
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

    public IPlayerAttribute PlayerAttribute
    {
        set
        {
            if (playerAttribute == null)
            {
                playerAttribute = value;
            }
        }
    }
    #endregion


    #region//-----private methods
    void SendMesOutSideOnInteractBtnPressed()
    {
        GameEvents.Sigton.InteractEventDictionary["onInteractBtnPressedWhenInteracting"].OnNext(new SubjectArg("ResourceCollect"));
    }
    #endregion
}
