using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

public class TimeSystemPresenter : MonoBehaviour
{
    [SerializeField]
    TimeSystemModel timeModel = new TimeSystemModel();
    [Header("the test and readonly data")]
    [SerializeField]
    bool IsDay;
    private void Awake()
    {
        timeModel.eclipsedTime = timeModel.dayLastTime;
        timeModel.isActive = true;
        timeModel.isDay.Value = true;
    }
    // Start is called before the first frame update
    void Start()
    {

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(x => timeModel.isActive)
            .Where(x => timeModel.isDay.Value)
            .Subscribe(x =>
            {
                timeModel.eclipsedTime = timeModel.eclipsedTime - 1;
                timeModel.eclipsedTime = Mathf.Clamp(timeModel.eclipsedTime, 0, timeModel.dayLastTime);
                if (timeModel.eclipsedTime <= 0)
                {
                    timeModel.isDay.Value = false;
                }
            });

        Observable.Interval(TimeSpan.FromSeconds(1))
            .Where(x => timeModel.isActive)
            .Where(x => !timeModel.isDay.Value)
            .Subscribe(x =>
            {
                timeModel.eclipsedTime = timeModel.eclipsedTime - 1;
                timeModel.eclipsedTime = Mathf.Clamp(timeModel.eclipsedTime, 0, timeModel.dayLastTime);
                if (timeModel.eclipsedTime <= 0)
                {
                    timeModel.isDay.Value = true;
                    timeModel.eclipsedDay++;
                }
            });

        timeModel.isDay
            .Subscribe(x =>
            {
                IsDay = x;
            });

        timeModel.isDay
            .Subscribe(x =>
            {
                if (x)
                {
                    timeModel.eclipsedTime = timeModel.dayLastTime;
                    AssertExtension.NotNullRun(GameEvents.Sigton.onDayStart, () =>
                    {
                        GameEvents.Sigton.onDayStart.Invoke();
                    });
                }
                else
                {
                    timeModel.eclipsedTime = timeModel.nightLastTime;
                    AssertExtension.NotNullRun(GameEvents.Sigton.onDayEnd, () =>
                    {
                        GameEvents.Sigton.onDayEnd.Invoke();
                    });
                }
            });
    }

}
