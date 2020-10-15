using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

[Serializable]
public class PlayerAttributeModel
{
    public ReactiveProperty<int> currentFatigue = new ReactiveProperty<int>();
    public ReactiveProperty<int> hunger = new ReactiveProperty<int>();
    [Header("-----疲劳值上限-----")]
    public int ceilingFatigue = 100;
    [Header("-----疲劳值下限-----")]
    public int floorFatige = 0;
    [Header("-----饥饿值上限-----")]
    public int ceilingHunger = 100;
 
}

