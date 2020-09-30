using UnityEngine;
using UniRx;
using System;

public class PlayerAttributePresenter : MonoBehaviour
{
    PlayerAttributeModel model = new PlayerAttributeModel();

    #region//Property Block
    public ReactiveProperty<int> Fatigue
    {
        get => model.currentFatigue;
        set
        {
            model.currentFatigue.Value = Mathf.Clamp(model.currentFatigue.Value, 0, model.ceilingFatigue.Value);
        }
    }
    public ReactiveProperty<int> Hunger
    {
        get => model.hunger;
        set
        {
            model.hunger.Value= Mathf.Clamp(model.hunger.Value, 0, model.ceilingHunger.Value);
        }
    }
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        GUIEvents.Singleton.Fatigue = model.currentFatigue;
        GUIEvents.Singleton.Hunger = model.hunger;

    }
    void Start()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(x =>
            {
                //model.currentFatigue.Value = model.currentFatigue.Value + 1;
            });
    }
}
