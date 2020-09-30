using UniRx;

[System.Serializable]
public class TimeSystemModel 
{
    public float eclipsedDay;
    public float eclipsedTime;
    public float dayLastTime;
    public float nightLastTime;
    public ReactiveProperty<bool> isDay = new ReactiveProperty<bool>();
    public bool isActive = true;
}
