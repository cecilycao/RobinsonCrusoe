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
        var config = GameConfig.Singleton.InteractionConfig;

        GameEvents.Sigton.onHungerReachMax
            .Subscribe(x =>
            {
                print("饥饿值达到最大");
            });

        GameEvents.Sigton.onFatigueReachMax
            .Subscribe(x =>
            {
                print("疲劳值达到最大");
            });

        GameEvents.Sigton.MechanismEventDictionary[MechanismEventTags.onDayTimeOut]
            .Subscribe(x =>
            {
                print("白天时间结束");
            });
    }
}