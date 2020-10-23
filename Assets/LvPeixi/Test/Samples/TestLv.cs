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
    }
}