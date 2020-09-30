using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

/// <summary>
/// Mediator负责多个系统之间协调、判定
/// </summary>
public class Mediator : MonoBehaviour
{
    static Mediator _instance;

    IInteractableNPC theCurrentInteractNPC;
    public static Mediator Sigton
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
    private void Start()
    {
        GameEvents.Sigton.onInteractStart += (IInteractable _interactObject) =>
        {
            if (_interactObject.InteractObjectType == "NPC")
            {


               
                  
            }
        };
    }
    /// <summary>
    /// 请求开始对话互动
    /// </summary>
    /// <param name="npc"></param>
    public void StartDialog(IInteractableNPC npc)
    {
     
        GameEvents.Sigton.onInteractStart.Invoke(npc);
        print("start interact");
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
            waitForKeyboardInteractSingle.Dispose();
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
        };
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
    /// <summary>
    /// 请求开始收集互动
    /// </summary>
    /// <param name="collector"></param>
    public void StartResourceCollect(IInteractableResourceCollector collector)
    {
        print("enter collect trigger");
        IDisposable waitForKeyboardInteractSingle = null;
        GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("按下E键收集资源");
        waitForKeyboardInteractSingle = Observable.EveryUpdate()
            .Where(x => Input.GetKeyDown(KeyCode.E))
            .Subscribe(x =>
            {
                collector.OnResourceCollectStart();
                AssertExtension.NotNullRun(GameEvents.Sigton.onResourceCollected, () =>
                {
                    GameEvents.Sigton.onResourceCollected.Invoke(collector.ResourceType,collector.ResourceAccount);
                });
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");

                GameEvents.Sigton.onInteractEnd();
                collector.OnResourceCollectEnd();
            });

        GameEvents.Sigton.onInteractEnd += () =>
        {
            waitForKeyboardInteractSingle.Dispose();
            GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
        };
    }
    /// <summary>
    /// 结束互动
    /// </summary>
    public void EndInteract()
    {
        GameEvents.Sigton.onInteractEnd();
    }
}
