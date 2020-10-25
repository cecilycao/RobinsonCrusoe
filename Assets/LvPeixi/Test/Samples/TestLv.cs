using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using UniRx;

public class TestLv : MonoBehaviour
{
    IDisposable test;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Sigton.onNPCSicked
             .Subscribe(x =>
             {
                 print("触发NPC生病事件");
             });
        GameEvents.Sigton.onPlayerSicked
            .Subscribe(x =>
            {
                print("触发玩家生病事件");
            });

        test =
            Observable.Timer(TimeSpan.FromSeconds(2))
            .Subscribe(x =>
            {
                test.Dispose();
            });


        OnPlayerContactStarted();

        OnContactEnd();

        OnInterctBtnPressed();

        OnInteractBtnReleased();

        OnInteractionEnd();

        SendMonologue();
    }
    void OnPlayerContactStarted()
    {
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.onPlayerContactStarted)
         .Subscribe(x =>
         {
             //print(x.senderSignature);
         });
    }

    void OnInterctBtnPressed()
    {
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.onInteractBtnPressed)
            .Subscribe(x =>
            {
                //print(x.senderSignature);
            });
    }

    void OnInteractBtnReleased()
    {
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.onInteractBtnReleased)
            .Subscribe(x =>
            {
                //print(x.senderSignature);
            });
    }

    void OnInteractionEnd()
    {
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.onInteractionCompleted)
            .Subscribe(x =>
            {
                //print(x.senderSignature);
            });
    }

    void OnContactEnd()
    {
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.onPlayerContactEnded)
            .Subscribe(x =>
            {
                //print(x.senderSignature);
            });
    }

    void SendMonologue()
    {
        //获取showMonologue事件subject
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.showMonologue)
            .Subscribe(x =>
            {
                print(x.senderSignature);
            });
    }
}