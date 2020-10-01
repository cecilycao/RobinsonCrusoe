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
    static IMediator _instance;

    IInteractableNPC theCurrentInteractNPC;
    /// <summary>
    /// 过滤掉多个请求
    /// </summary>
    bool IsAtInteractState = false;
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

                    GameEvents.Sigton.onInteractEnd();
                    collector.OnResourceCollectEnd();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                waitForKeyboardInteractSingle.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    public void StartAddIsland(IslandBuilder builder)
    {
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;

            IDisposable waitForKeyboardInteractSingle = null;
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键创建岛屿");
            waitForKeyboardInteractSingle = Observable.EveryUpdate()
                .Where(x => Input.GetKeyDown(KeyCode.E))
                .Subscribe(x =>
                {
                    builder.OnIslandBuild();
                    AssertExtension.NotNullRun(GameEvents.Sigton.onIslandCreated, () =>
                    {
                        GameEvents.Sigton.onIslandCreated.Invoke();
                    });
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");

                    GameEvents.Sigton.onInteractEnd();
                    builder.OnIslandBuildEnd();
                });

            GameEvents.Sigton.onInteractEnd += () =>
            {
                IsAtInteractState = false;
                waitForKeyboardInteractSingle.Dispose();
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            };
        }
    }
    public void StartRestoreIsland(IInteractableIsland island)
    {
 
        if (!IsAtInteractState)
        {
            IsAtInteractState = true;

            //check player have enough resource?
            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            var playerBuildingMaterial = inventory.BuildingMaterial.Value;
            print(playerBuildingMaterial < island.MaterialCost);
            if (playerBuildingMaterial < island.MaterialCost)
            {
                IsAtInteractState = false;
                return;
            }
            //listen the interact key pressed event
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键修复浮岛");
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
    public void StartProcessFood()
    {
    }
    public void EndInteract()
    {
        GameEvents.Sigton.onInteractEnd();
    }
}
