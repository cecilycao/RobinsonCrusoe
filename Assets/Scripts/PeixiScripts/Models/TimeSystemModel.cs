using UniRx;
using UnityEngine;

[System.Serializable]
public class TimeSystemModel 
{
    public ReactiveProperty<float> dayCount = new ReactiveProperty<float>(1);
    public ReactiveProperty<float> timeCountdown = new ReactiveProperty<float>(0);
    public ReactiveProperty<bool> isDay = new ReactiveProperty<bool>();
    [Header("-----白天时长-----")]
    public float dayLastTime;
    [Header("-----夜晚时长-----")]
    public float nightLastTime;
    public bool isActive = true;
}
