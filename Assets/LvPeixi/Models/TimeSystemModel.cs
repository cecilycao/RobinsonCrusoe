using UniRx;

[System.Serializable]
public class TimeSystemModel 
{
    public ReactiveProperty<float> dayCount = new ReactiveProperty<float>(0);
    public ReactiveProperty<float> timeCountdown = new ReactiveProperty<float>(0);
    public ReactiveProperty<bool> isDay = new ReactiveProperty<bool>();
    public float dayLastTime;
    public float nightLastTime;
    public bool isActive = true;
}
