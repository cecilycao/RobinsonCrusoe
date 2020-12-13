using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;
using UnityEngine.Events;

public class TimeLine : MonoBehaviour
{
    float time;
    float day;
    public TimeEvent[] events;

    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Sigton.timeSystem
        .Subscribe(_data =>
        {
            time = _data.TimeCountdown;
            day = _data.DayCount;
            foreach(TimeEvent e in events)
            {
                if(e.day == day && e.time == time)
                {
                    Debug.Log("Time for event " + e.timeEvent.ToString());
                    e.invokeEvent();
                }
            }
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

[Serializable]
public class TimeEvent
{
    public float day;
    public float time;
    public TimeEventName timeEvent;
    
    public void invokeEvent()
    {
        switch (timeEvent)
        {
            case TimeEventName.ON_RAIN_START:
                GameEvents.Sigton.OnRainStart.Invoke();
                return;
            case TimeEventName.ON_RAIN_END:
                GameEvents.Sigton.OnRainEnd.Invoke();
                return;
            case TimeEventName.ON_STORM_START:
                GameEvents.Sigton.OnStormStart.Invoke();
                return;
            case TimeEventName.ON_NPC_ISLAND_APPEAR:
                GameEvents.Sigton.onNPCIslandAppear.Invoke();
                return;
            case TimeEventName.ON_NPC_ISLAND_COMBINED:
                GameEvents.Sigton.onNPCIslandCombined.Invoke();
                return;
        }
    }
}

public enum TimeEventName
{
    ON_RAIN_START,
    ON_RAIN_END,
    ON_STORM_START,
    ON_STORM_END,
    ON_NPC_ISLAND_APPEAR,
    ON_NPC_ISLAND_COMBINED
}