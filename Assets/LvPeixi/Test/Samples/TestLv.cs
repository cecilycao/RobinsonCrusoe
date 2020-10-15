using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using UniRx;

public class TestLv : MonoBehaviour
{
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
    }
}