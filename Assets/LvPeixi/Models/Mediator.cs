using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

/// <summary>
/// Mediator负责多个系统之间协调、判定
/// </summary>
public class Mediator : MonoBehaviour,IMediator
{
    #region//private variables
    static IMediator _instance;
    IInteractableNPC theCurrentInteractNPC;
    /// <summary>
    /// 过滤掉多个密集请求
    /// </summary>
    bool IsAtInteractState = false;
    #endregion

    #region//initialize
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
    }
    #endregion

    #region//IMediator implement
    public void StartDialog(IInteractableNPC npc)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;

            GameEvents.Sigton.onInteractStart.Invoke(npc);
            IDisposable waitForKeyboardInteractSingle = null;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键对话");

            waitForKeyboardInteractSingle = Observable.EveryUpdate()
                .Where(x => Input.GetKeyDown(KeyCode.E))
                .Subscribe(x =>
                {
                    theCurrentInteractNPC = npc;
                    DialogManager.Singelton.StartDialog(npc.NPCName);
                    npc.OnDialogStart();
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
    public void EndDialog()
    {
        if (theCurrentInteractNPC!= null)
        {
            theCurrentInteractNPC.OnDialogEnd();
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

            IDisposable waitForKeyboardInteractSingle = null;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键收集资源");
            waitForKeyboardInteractSingle = Observable.EveryUpdate()
                .Where(x => Input.GetKeyDown(KeyCode.E))
                .Subscribe(x =>
                {
                    collector.OnResourceCollectStart();
                    AssertExtension.NotNullRun(GameEvents.Sigton.onResourceCollected, () =>
                    {
                        GameEvents.Sigton.onResourceCollected.Invoke(collector.ResourceType, collector.ResourceAccount);
                    });
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");

                    collector.OnResourceCollectEnd();
                    GameEvents.Sigton.onInteractEnd();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                waitForKeyboardInteractSingle.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    public void StartIslandBuild(IInteractableIsland island)
    {
       
    }
    public void StartRestoreIsland(IInteractableIsland island)
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
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键建造浮岛");
            IDisposable waitForKeyboardInteractEvent = null;
            waitForKeyboardInteractEvent = InputSystem.Singleton.OnInteractBtnPressed
                .Subscribe(x =>
                {
                    island.OnIslandRestoreStart();
                    inventory.BuildingMaterial.Value -= island.MaterialCost;

                    island.OnIslandRestoreEnd();
                    GameEvents.Sigton.onInteractEnd();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                waitForKeyboardInteractEvent.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    public void StartProcessFood(IFoodProcess foodProcess)
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
            //listen the interact key pressed event
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键生产食物");
            IDisposable waitForKeyboardInteractEvent = null;
            waitForKeyboardInteractEvent = InputSystem.Singleton.OnInteractBtnPressed
                .Subscribe(x =>
                {
                    foodProcess.OnStartProcessFood();
                    inventory.FoodMaterial.Value -= foodProcess.Cost;
                    attr.Hunger.Value -= foodProcess.HungerRestore;
                    foodProcess.OnEndProcessFood();
                    GameEvents.Sigton.onInteractEnd();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                waitForKeyboardInteractEvent.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    public void EndInteract()
    {
        GameEvents.Sigton.onInteractEnd();
    }
    #endregion
}
