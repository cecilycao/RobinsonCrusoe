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

    IDisposable watchInteractBtnPressed = null;
    IDisposable watchInteractBtnReleased = null;
    IDisposable watchInteractCompleted = null;

    private Dictionary<string, float> interactConfig;

    [Header("-----TEST BLOCK-----")]
    [Header("主动收集的次数")]
    public int positiveCollectPerformTime = 0;
    [Header("玩家正在接触的交互道具")]
    [SerializeField]
    private string theInteractObject = "None";
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
        GameEvents.Sigton.RegisterEvent(EventDictionaryType.InteractEvent, InteractEventTags.interact_onInteractionCompleted);
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
            theInteractObject = "NPC";
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
                theInteractObject = "None";
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
            theInteractObject = "PositiveCollector";
            if (CheckPlayerFatigue())
            {
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("我太累了，不想干");
                Observable.Timer(TimeSpan.FromSeconds(1))
                    .First()
                    .Subscribe(x =>
                    {
                        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    });
                return;
            }

            IsAtInteractState = true;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键收集资源");
            collector.ShowIcon();
        
            watchInteractBtnPressed = InputSystem.Singleton.OnInteractBtnPressed
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
                            //GameEvents.Sigton.onResourceCollected.Invoke(collector.ResourceType, collector.ResourceAccount_Food);
                            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();

                            int _food = (int)GameConfig.Singleton.InteractionConfig[InteractConfigKeys.posCollect_foodMat_defaut];
                            int _build = (int)GameConfig.Singleton.InteractionConfig[InteractConfigKeys.posCollect_buildingMat_defaut];
                            inventory.FoodMaterial.Value += _food;
                            inventory.BuildingMaterial.Value += _build;
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

                    GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.interact_onInteractionCompleted)
                        .OnNext(new SubjectArg(this.name,
                            InteractableObjectType.PositiveCollect
                        ));

                    GameEvents.Sigton.onInteractEnd.Invoke();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                playerInteract.PlayerEndInteraction();
                if (watchInteractBtnPressed != null)
                {
                    watchInteractBtnPressed.Dispose();
                }
                theInteractObject = "None";
                collector.HideIcon();
                ReleaseAllWatch();
            };
        }
    }
    /// <summary>
    /// 被动收集，只需要按住不放即可
    /// </summary>
    /// <param name="collector"></param>
    public void StartInteraction(INegativeResourceCollector collector)
    {
         if (!IsAtInteractState)
        {
            theInteractObject = "NegativeCollector";
            if (CheckPlayerFatigue())
            {
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("我太累了，不想干");
                Observable.Timer(TimeSpan.FromSeconds(1))
                    .First()
                    .Subscribe(x =>
                    {
                        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    });
                return;
            }

            IsAtInteractState = true;
            collector.ShowIcon();
            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键收集资源");

            float buildIslandCostTime = interactConfig["addIslandTimeCost"];

            watchInteractBtnPressed = InputSystem.Singleton.OnInteractBtnPressed
                .Subscribe(x =>
                {
                    SendMesOutSideOnInteractBtnPressed();
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("正在收集资源");
                    collector.HideIcon();
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(buildIslandCostTime);
                    //AudioManager.Singleton.PlayAudio("Interact_islandBuilding");
                    collector.StartInteract();
                    playerInteract.PlayerStartInteraction(PlayerInteractionType.Collect);
                });

            watchInteractCompleted =
            InputSystem.Singleton.OnInteractBtnPressed
                .Delay(TimeSpan.FromSeconds(buildIslandCostTime))
                .First()
                .Subscribe(x =>
                {
                   
                    //-----set player attribute
                    int fatigueIncrease = (int)interactConfig["negativeCollectFatigueIncrease"];
                    playerAttribute.Fatigue.Value += fatigueIncrease;

                    var _hungerChange = (int)interactConfig["interact_negativeCollect_hungerDecrea_default"];
                    playerAttribute.Hunger.Value += _hungerChange;

                    int _build = (int)GameConfig.Singleton.InteractionConfig[InteractConfigKeys.negCollect_buildingMat_default];
                    int _food = (int)GameConfig.Singleton.InteractionConfig[InteractConfigKeys.negCollect_foodMat_default];

                    inventory.BuildingMaterial.Value += _build;
                    inventory.FoodMaterial.Value += _food;

                    collector.EndInteract(true);

                    SendMesOutOnInteractCompleted(new SubjectArg("msg from negcollect", InteractableObjectType.NegativeCollect));
                    GameEvents.Sigton.onInteractEnd();
                });

             watchInteractBtnReleased =
             InputSystem.Singleton.OnInteractBtnReleased
                .First()
                .Subscribe(x =>
                {
                    AudioManager.Singleton.PauseAudio("Interact_islandBuilding");
                    GameEvents.Sigton.onInteractEnd();
                    SendMesOutSideInteractBtnReleased("Cancel_building_island");
                    AudioManager.Singleton.PlayAudio("Interact_build_restoreIsland_processFoodComplete");
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                collector.EndInteract(false);
                playerInteract.PlayerEndInteraction();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                collector.HideIcon();
                theInteractObject = "None";
                ReleaseAllWatch();

                

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
            theInteractObject = "IslandBuilder";
            if (CheckPlayerFatigue())
            {
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("我太累了，不想干");
                Observable.Timer(TimeSpan.FromSeconds(1))
                    .First()
                    .Subscribe(x =>
                    {
                        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    });
                return;
            }

            IsAtInteractState = true;
            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            var playerBuildingMaterial = inventory.BuildingMaterial.Value;
            if (playerBuildingMaterial < builder.MaterialCost)
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("创建浮岛所需的建材不够");
                return;
            }

            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键创建岛屿");
            builder.ShowIcon();

            float buildIslandCostTime = interactConfig["addIslandTimeCost"];

            watchInteractBtnPressed =
            InputSystem.Singleton.OnInteractBtnPressed
                .Subscribe(x =>
                {
                    SendMesOutSideOnInteractBtnPressed();
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("正在建造新浮岛");
                    builder.HideIcon();
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(buildIslandCostTime);
                    AudioManager.Singleton.PlayAudio("Interact_islandBuilding");
                    builder.StartInteract();
                    playerInteract.PlayerStartInteraction(PlayerInteractionType.Collect);
                });

            watchInteractCompleted =
            InputSystem.Singleton.OnInteractBtnPressed
                .Delay(TimeSpan.FromSeconds(buildIslandCostTime))
                .Subscribe(x =>
                {
                    AudioManager.Singleton.PauseAudio("Interact_islandBuilding");
                    //-----set player attribute
                    int fatigueIncrease = (int)interactConfig["addIslandFatigueIncrease"];
                    playerAttribute.Fatigue.Value += fatigueIncrease;

                    var _hungerChange = (int)interactConfig["interact_addIsland_hungerDecrea_default"];
                    playerAttribute.Hunger.Value += _hungerChange;

                    inventory.BuildingMaterial.Value -= builder.MaterialCost;

                    builder.EndInteract(true);

                    SendMesOutOnInteractCompleted(new SubjectArg(name, InteractableObjectType.IslandBuilder));

                    GameEvents.Sigton.onInteractEnd();
                });

            watchInteractBtnReleased = 
            InputSystem.Singleton.OnInteractBtnReleased
                .Subscribe(x =>
                {
                    AudioManager.Singleton.PauseAudio("Interact_islandBuilding");
                    GameEvents.Sigton.onInteractEnd();
                    //GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                    SendMesOutSideInteractBtnReleased("Cancel_building_island");
                    AudioManager.Singleton.PlayAudio("Interact_build_restoreIsland_processFoodComplete");
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                builder.EndContact();
                playerInteract.PlayerEndInteraction();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                builder.HideIcon();
                theInteractObject = "None";
                ReleaseAllWatch();
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
            theInteractObject = "FoodProcessPlant";
       
            IsAtInteractState = true;
            var attr = FindObjectOfType<PlayerAttributePresenter>();
            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            
            //check player has enough food material?
            var hasEnoughFoodMaterial = inventory.FoodMaterial.Value >= 4;
            if (!hasEnoughFoodMaterial)
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("没有足够的食材加工食物");
                Observable.Timer(TimeSpan.FromSeconds(2))
                    .First()
                    .Subscribe(x =>
                    {
                        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    });
            }
  
            if (foodProcess.HasFood)
            {
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键吃食物");
                foodProcess.ShowIcon();
                watchInteractBtnPressed = 
                InputSystem.Singleton.OnInteractBtnPressed
                    .Subscribe(x =>
                    {
                        foodProcess.HideIcon();
                        int _hungerRes = (int)interactConfig["eatFood_hungerDec_default"];
                        attr.Hunger.Value -= _hungerRes;
                        attr.Fatigue.Value -= 10;
                        foodProcess.EndInteract(false);
                        GameEvents.Sigton.onInteractEnd();
                    });
            }
            else
            {
                if (CheckPlayerFatigue())
                {
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("我太累了，不想干");
                    Observable.Timer(TimeSpan.FromSeconds(1))
                        .First()
                        .Subscribe(x =>
                        {
                            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                        });
                    return;
                }
                if (inventory.FoodMaterial.Value < 4)
                {
                    return;
                }
                float processFoodCostTime = interactConfig["processFoodTimeCost"];
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按住E键生产食物");
                foodProcess.ShowIcon();
                //listen the interact key pressed event
                watchInteractBtnPressed =
                InputSystem.Singleton.OnInteractBtnPressed
                    .First()
                    .Subscribe(x =>
                    {
                        print("start process food");
                        SendMesOutSideOnInteractBtnPressed();
                        foodProcess.HideIcon();
                        foodProcess.StartInteract();
                        GUIEvents.Singleton.InteractionProgressBar.OnNext(processFoodCostTime);
                        AudioManager.Singleton.PlayAudio("Interact_processingFood");
                        //GameEvents.Sigton.onInteractEnd();
                    });

                watchInteractCompleted =
                InputSystem.Singleton.OnInteractBtnPressed
                    .Delay(TimeSpan.FromSeconds(processFoodCostTime))
                    .Subscribe(x =>
                    {
                        inventory.FoodMaterial.Value -= 4;
                        int fatigueIncrease = (int)interactConfig["processFoodFatigeIncrease"];
                        playerAttribute.Fatigue.Value += fatigueIncrease;
                        //var _hungerChanged = (int)interactConfig["interact_processFood_hungerDecrea_default"];
                        //playerAttribute.Hunger.Value -= _hungerChanged;
                        //AudioManager.Singleton.PlayAudio("Interact_build_restoreIsland_processFoodComplete");
                        SendMesOutOnInteractCompleted(new SubjectArg("msg from food process", InteractableObjectType.FoodProcessPlant));
                        foodProcess.EndInteract(true);
                        GameEvents.Sigton.onInteractEnd();
                    });

                watchInteractBtnReleased =
                InputSystem.Singleton.OnInteractBtnReleased
                    .First()
                    .Subscribe(x =>
                    {
                        //GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                        GameEvents.Sigton.onInteractEnd();
                        AudioManager.Singleton.PauseAudio("Interact_processingFood");
                        //foodProcess.EndInteract(true);         
                    });
            }
            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                foodProcess.HideIcon();
                theInteractObject = "None";
                ReleaseAllWatch();
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
            if (CheckPlayerFatigue())
            {
                theInteractObject = "Island";
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("我太累了，不想干");
                Observable.Timer(TimeSpan.FromSeconds(1))
                    .First()
                    .Subscribe(x =>
                    {
                        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    });
                return;
            }
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
           
            watchInteractBtnPressed =
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
                });

            watchInteractCompleted =
            InputSystem.Singleton.OnInteractBtnPressed
                .Delay(TimeSpan.FromSeconds(restoreIslandCostTime))
                .Subscribe(x =>
                {
                    AudioManager.Singleton.PauseAudio("Interact_islandRestoring");
                    int _fatigueChange = (int)interactConfig[InteractConfigKeys.restoreIsland_fatigueChange_default];
                    playerAttribute.Fatigue.Value += _fatigueChange;
                    var _hungerChange = (int)interactConfig[InteractConfigKeys.restoreIsland_hungerChange_default];
                    playerAttribute.Hunger.Value += _hungerChange;

                    inventory.BuildingMaterial.Value -= island.MaterialCost;
                    AudioManager.Singleton.PlayAudio("Interact_build_restoreIsland_processFoodComplete");
                    GameEvents.Sigton.onInteractEnd();
                    island.EndInteract(true);
                });

            watchInteractBtnReleased =
            InputSystem.Singleton.OnInteractBtnReleased
                .First()
                .Subscribe(x =>
                {
                    GUIEvents.Singleton.InteractionProgressBar.OnNext(0);
                    GameEvents.Sigton.onInteractEnd();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                island.HideIcon();
                theInteractObject = "None";
                ReleaseAllWatch();
            };
        }
    }
    public void OpenDiary(Diary diary)
    {
        if (!IsAtInteractState)
        {
            theInteractObject = "Diary";
            IsAtInteractState = true;

            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键休息");
            diary.ShowIcon();

            watchInteractBtnPressed = Observable.EveryUpdate()
                .Where(x => Input.GetKeyDown(KeyCode.E))
                .Subscribe(x =>
                {
                    diary.OnDiaryOpen();
                    //playerInteract.PlayerStartInteraction(PlayerInteractionType.Dialog);
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                    diary.HideIcon();
                    watchInteractBtnPressed.Dispose();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                diary.OnDiaryClose();
                IsAtInteractState = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
                diary.HideIcon();
                theInteractObject = "None";
                ReleaseAllWatch();
            };
        }
    }

    public void playerSick()
    {
        playerInteract.PlayerStartInteraction(PlayerInteractionType.Dialog);
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

    void SendMesOutSideInteractBtnReleased(string mes)
    {
        string _eventKey = InteractEventTags.onInteractBtnReleasedWhenInteracting;
        GameEvents.Sigton.InteractEventDictionary[_eventKey].OnNext(new SubjectArg(mes));
    }
    void SendMesOutOnInteractCompleted(SubjectArg subjectArg)
    {
        var _eventKey = InteractEventTags.interact_onInteractionCompleted;
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(_eventKey).OnNext(subjectArg);
    }

    void ReleaseAllWatch()
    {
        if (watchInteractBtnPressed != null)
        {
            watchInteractBtnPressed.Dispose();
        }
        if (watchInteractBtnReleased != null)
        {
            watchInteractBtnReleased.Dispose();
        }
        if (watchInteractCompleted != null)
        {
            watchInteractCompleted.Dispose();
        }
    }

    bool CheckPlayerFatigue()
    {
        return playerAttribute.Fatigue.Value >= 100;
    }
    #endregion
}

public enum InteractableObjectType
{
    NPC,
    PositiveCollect,
    NegativeCollect,
    Island,
    IslandBuilder,
    FoodProcessPlant
}
