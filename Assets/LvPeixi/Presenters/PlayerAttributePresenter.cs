using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using System;

public class PlayerAttributePresenter : MonoBehaviour,IPlayerAttribute
{
    [SerializeField]
    PlayerAttributeModel model = new PlayerAttributeModel();

    bool isAccmulatingFatigue = false;
    IDisposable onAccumulateComplete = null;
    IDisposable accumulateFatigue = null;
    IDisposable accumulateHunger = null;
    Dictionary<string,float> config;

    [Header("-----Test block-----")]
    [SerializeField]
    int t_Hunger;
    [SerializeField]
    int t_Fatigue;

    #region//Property Block
    public ReactiveProperty<int> Fatigue
    {
        get => model.currentFatigue;
        set
        {
            model.currentFatigue.Value = Mathf.Clamp(model.currentFatigue.Value, 0, model.ceilingFatigue);
            Debug.Log("currentFatigue: " + model.currentFatigue.Value);
            if (model.currentFatigue.Value >= model.ceilingFatigue)
            {
                GameEvents.Sigton.onFatigueReachMax.OnNext(model.currentFatigue.Value);
            }
        }
    }
    public ReactiveProperty<int> Hunger
    {
        get => model.hunger;
        set
        {
            model.hunger.Value = Mathf.Clamp(model.hunger.Value, 0, 100);
        }
    }
    #endregion

    private void OnEnable()
    {
        
    }
    void Start()
    {

        Configurate();
        #region-----When time is out, start to accumulate fatigue
        GameEvents.Sigton.timeSystem
            .Delay(TimeSpan.FromSeconds(1))
            .Where(y => y.TimeCountdown == 0 && !isAccmulatingFatigue && y.IsDay)
            .Subscribe(x =>
            {
                AccumulateFatigue();
            });
        #endregion

        OnDayStart();

        OnDayEnd();

        TestProperty();

        OnFatigueChanged();

        OnHungerChanged();

    }
    void AccumulateFatigue()
    {
        if (isAccmulatingFatigue)
        {
            return;
        }
        isAccmulatingFatigue = true;
        int _fatigueAccmuSpeed = (int)GameConfig.Singleton.PlayerConfig["fatigueIncreaseWhenTimeOut"];

        accumulateFatigue = Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(y => model.currentFatigue.Value < 100)
            .Subscribe(x =>
            {
                model.currentFatigue.Value = model.currentFatigue.Value + _fatigueAccmuSpeed;
                if (model.currentFatigue.Value >= 100)
                {
                    GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("我已经筋疲力尽了");
                    accumulateFatigue.Dispose();
                }
            }
            );
        onAccumulateComplete = model.currentFatigue
            .Where(x => x == 100)
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
            model.poison.Value += _fatigeFloorIncrease;
            model.floorFatige += _fatigeFloorIncrease;
            model.currentFatigue.Value = model.floorFatige;

            HungerDec();
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
            AssertExtension.NotNullRun(accumulateHunger, () =>
            {
                accumulateHunger.Dispose();
            });
        };
    }

    void Configurate()
    {
        config = GameConfig.Singleton.PlayerConfig;
        model.hunger.Value = (int)config["playerAttr_hungerValue_start"];
        model.currentFatigue.Value = (int)config["playerAttr_fatigueValue_start"];

        GUIEvents.Singleton.Hunger = model.hunger;
        GUIEvents.Singleton.Fatigue = model.currentFatigue;
        GameEvents.Sigton.RegisterEvent(GameEvents.EventDictionaryType.PlayerEvent, PlayerEventTags.onPoisonChanged, model.poison);
        Mediator.Sigton.PlayerAttribute = this;
    }

    void HungerDec()
    {
        accumulateHunger = Observable.Interval(TimeSpan.FromSeconds(3))
            .Subscribe(x =>
            {
                var hungerAccumulateSpeed = (int)config["playerAttr_defaultHungerAccumulateSpeed"];
                model.hunger.Value += hungerAccumulateSpeed;
            });
    }

    void TestProperty()
    {
        model.currentFatigue
            .Subscribe(x =>
            {
                t_Fatigue = x;
            });
        model.hunger
            .Subscribe(x =>
            {
                t_Hunger = x;
            });
    }

    void OnFatigueChanged()
    {
        model.currentFatigue
            .Subscribe(x =>
            {
                if (x>=100)
                {
                    GameEvents.Sigton.onFatigueReachMax.OnNext(x);
                }
            });
    }

    void OnHungerChanged()
    {
        model.hunger
            .Where(x=>x==0)
            .Subscribe(x =>
            {
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("我太饿了，需要食物");
            });
        model.hunger
            .Where(x => x == 0)
            .Delay(TimeSpan.FromSeconds(2))
            .Subscribe(x =>
            {
                GUIEvents.Singleton.BroadcastInteractTipMessage.OnNext("");
            });

        model.hunger
            .Where(x => x == 0)
            .Delay(TimeSpan.FromSeconds(15))
            .Subscribe(x =>
            {
                if (model.hunger.Value == 0)
                {
                    GameEvents.Sigton.onDayEnd.Invoke();
                }
            });
    }
}
