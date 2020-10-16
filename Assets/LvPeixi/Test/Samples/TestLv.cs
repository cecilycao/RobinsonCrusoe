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
        GUIEvents.Singleton.Fatigue
            .Where(y => y == 0)
            .Subscribe(x =>
            {
                ///执行代码
            });

  

        Observable.EveryUpdate()
            .Where(x => Input.GetKeyDown(KeyCode.T))
            .Subscribe(x =>
            {
                
            });
    }
}