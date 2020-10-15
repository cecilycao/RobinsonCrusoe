using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TimeSystemPresenter : MonoBehaviour, ITimeSystemData
{
    [SerializeField]
    TimeSystemModel timeModel = new TimeSystemModel();

    #region//-----implemet interace-----
    public float DayCount { get => timeModel.dayCount.Value; }
    public float TimeCountdown { get => timeModel.timeCountdown.Value; }
    public bool IsDay { get => timeModel.isDay.Value; }
    #endregion


    private void Awake()
    {
        timeModel.timeCountdown = new ReactiveProperty<float>(timeModel.dayLastTime);
        timeModel.isActive = true;
        timeModel.isDay.Value = true;
    }
    // Start is called before the first frame update
    void Start()
    {
        DayCountStream();

        NightCountStream();

        SendTimeSystemInfoToGuiEventsStream();

        OnDayStart();
    }
    #region-----private methods-----
    void DayCountStream()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
        .Where(x => timeModel.isActive)
        .Where(x => timeModel.isDay.Value)
        .Subscribe(x =>
        {
            timeModel.timeCountdown.Value = timeModel.timeCountdown.Value - 1;
            timeModel.timeCountdown.Value = Mathf.Clamp(timeModel.timeCountdown.Value, 0, timeModel.dayLastTime);
            if (timeModel.timeCountdown.Value <= 0)
            {
                //timeModel.isDay.Value = false;
                //timeModel.timeCountdown.Value = timeModel.nightLastTime;
            }
        });
    }

    void NightCountStream()
    {
        Observable.Interval(TimeSpan.FromSeconds(1))
       .Where(x => timeModel.isActive)
       .Where(x => !timeModel.isDay.Value)
       .Subscribe(x =>
       {
           timeModel.timeCountdown.Value = timeModel.timeCountdown.Value - 1;
           timeModel.timeCountdown.Value = Mathf.Clamp(timeModel.timeCountdown.Value, 0, timeModel.dayLastTime);
           if (timeModel.timeCountdown.Value <= 0)
           {
               timeModel.isDay.Value = true;
               timeModel.dayCount.Value++;
               timeModel.timeCountdown.Value = timeModel.dayLastTime;
           }
       });
    }

    void OnDayStateChanged()
    {
        timeModel.isDay
          .Where(x=>timeModel.isActive)
          .Subscribe(x =>
          {
              if (x)
              {
                  timeModel.timeCountdown.Value = timeModel.dayLastTime;
                  AssertExtension.NotNullRun(GameEvents.Sigton.onDayStart, () =>
                  {
                      GameEvents.Sigton.onDayStart.Invoke();
                  });
              }
              else
              {
                  timeModel.timeCountdown.Value = timeModel.nightLastTime;
                  AssertExtension.NotNullRun(GameEvents.Sigton.onDayEnd, () =>
                  {
                      GameEvents.Sigton.onDayEnd.Invoke();
                  });
              }
          });
    }

    void SendTimeSystemInfoToGuiEventsStream()
    {
        timeModel.dayCount
       .Subscribe(x =>
        {
            GameEvents.Sigton.timeSystem.OnNext(this);
        });
        timeModel.timeCountdown
         .Subscribe(x =>
         {
             GameEvents.Sigton.timeSystem.OnNext(this);
         }); 
    }

    void OnDayStart()
    {
        timeModel.dayCount.Value++;
    }
    #endregion
}
