using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class TestMediator : MonoBehaviour
{
    public IObservable<Func<int, string>> getIslandType0;
    public Func<int, string> getIslandType1;
}
