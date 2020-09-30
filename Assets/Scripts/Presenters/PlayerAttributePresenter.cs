using UnityEngine;
using UniRx;
using System;

public class PlayerAttributePresenter : MonoBehaviour
{
    PlayerAttributeModel model = new PlayerAttributeModel();

    #region//Property Block
    public ReactiveProperty<int> Vitality
    {
        get => model.currentVitality;
        set
        {
            var _vitality = model.currentVitality.Value;
            _vitality = Mathf.Clamp(_vitality, 0, model.ceilingVitality.Value);
        }
    }
    public ReactiveProperty<int> Hunger
    {
        get => model.hunger;
        set
        {
            var _hunger = model.hunger.Value;
            _hunger = Mathf.Clamp(_hunger, 0, model.ceilingHunger.Value);
        }
    }
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        GUIEvents.Singleton.Vitality = model.currentVitality;
    }
    void Start()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
            .Subscribe(x =>
            {
                model.currentVitality.Value = model.currentVitality.Value + 1;
            });
    }
}
