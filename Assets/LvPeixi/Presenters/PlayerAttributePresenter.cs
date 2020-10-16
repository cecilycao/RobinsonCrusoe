using UnityEngine;
using UniRx;
using System;

public class PlayerAttributePresenter : MonoBehaviour,IPlayerAttribute
{
    [SerializeField]
    PlayerAttributeModel model = new PlayerAttributeModel();

    bool isAccmulatingFatigue = false;
    IDisposable onAccumulateComplete = null;
    IDisposable accumulateFatigue = null;
    #region//Property Block
    public ReactiveProperty<int> Fatigue
    {
        get => model.currentFatigue;
        set
        {
            model.currentFatigue.Value = Mathf.Clamp(model.currentFatigue.Value, 0, model.ceilingFatigue);
        }
    }
    public ReactiveProperty<int> Hunger
    {
        get => model.hunger;
        set
        {
            model.hunger.Value= Mathf.Clamp(model.hunger.Value, 0, model.ceilingHunger);
        }
    }
    #endregion
    // Start is called before the first frame update
    private void Awake()
    {
        GUIEvents.Singleton.Fatigue = model.currentFatigue;
        GUIEvents.Singleton.Hunger = model.hunger;
        Mediator.Sigton.PlayerAttribute = this;
    }
    void Start()
    {
        #region-----When time is out, start to accumulate fatigue
        GameEvents.Sigton.timeSystem
            .Delay(TimeSpan.FromSeconds(1))
            .Where(y => y.TimeCountdown == 0 && !isAccmulatingFatigue && y.IsDay)
            .Subscribe(x =>
            {
                print(x.IsDay);
                AccumulateFatigue();
            });
        #endregion

        OnDayStart();

        OnDayEnd();
    }
    void AccumulateFatigue()
    {
        isAccmulatingFatigue = true;
        print("is accumulating fatigue");
        int _fatigueAccmuSpeed = (int)GameConfig.Singleton.PlayerConfig["fatigueIncreaseWhenTimeOut"];

        accumulateFatigue = Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(y => model.currentFatigue.Value < model.ceilingFatigue)
            .Subscribe(x =>
            {
                model.currentFatigue.Value = model.currentFatigue.Value + _fatigueAccmuSpeed;
                if (model.currentFatigue.Value >= model.ceilingFatigue)
                {
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("我已经筋疲力尽了");
                    accumulateFatigue.Dispose();
                }
            }
            );
        onAccumulateComplete = model.currentFatigue
            .Where(x => x == model.ceilingFatigue)
            .Delay(TimeSpan.FromSeconds(1))
            .First()
            .Subscribe(y =>
            {
                isAccmulatingFatigue = false;
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            });
    }

    void OnDayStart()
    {
        GameEvents.Sigton.onDayStart += () =>
        {
            var _fatigeFloorIncrease = (int)GameConfig.Singleton.PlayerConfig["fatigueInceasePerDay"];
            model.floorFatige += _fatigeFloorIncrease;
            model.currentFatigue.Value = model.floorFatige;
        };
    }

    void OnDayEnd()
    {
        GameEvents.Sigton.onDayEnd += () =>
        {
            isAccmulatingFatigue = false;
            model.currentFatigue.Value = 0;

            if (onAccumulateComplete != null)
            {
                onAccumulateComplete.Dispose();
            }
            if (accumulateFatigue != null)
            {
                accumulateFatigue.Dispose();
            }
        };
    }
}
