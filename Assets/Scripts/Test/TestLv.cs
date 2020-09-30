using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using System;
using UniRx;

public class TestLv : MonoBehaviour
{

    public int n = 0;
    TestMediator testMediater;
    // Start is called before the first frame update
    void Start()
    {
        testMediater = GetComponent<TestMediator>();

        #region//Plan0
        Observable.EveryUpdate()
        .Where(x => Input.GetKeyDown(KeyCode.Q))
        .Subscribe(x =>
        {
             print("press key Q,perform plan0");
            testMediater.getIslandType0.Subscribe(y =>
            {
                var sdf = y.Invoke(n);
                print(sdf);
            });
        });
        #endregion

        #region//Plan1
        Observable.EveryUpdate()
            .Where(x => Input.GetKeyDown(KeyCode.W))
            .Subscribe(x =>
            {
                print("press key W,perform plan1");
                var _temp = testMediater.getIslandType1.Invoke(n);
                print(_temp);
            });
        #endregion

    }
}
