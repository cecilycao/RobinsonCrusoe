using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[Serializable]
public class PlayerAttributeModel
{
    public ReactiveProperty<int> currentFatigue = new ReactiveProperty<int>();
    public ReactiveProperty<int> ceilingFatigue = new ReactiveProperty<int>();
    public ReactiveProperty<int> hunger = new ReactiveProperty<int>();
    public ReactiveProperty<int> ceilingHunger = new ReactiveProperty<int>();
}

