using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class Legislator : MonoBehaviour
{
    // Start is called before the first frame update
    private void Awake()
    {
        
    }
    void WatchGameEnd()
    {
        GameEvents.Sigton.onGameEnd += () =>
        {
            GameEnd();
        };

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount > 16)
            .Subscribe(x =>
            {
                GameEnd();
            });
    }
    void GameEnd()
    {
        int coreIsland = IslandManager.Instance.m_islandSet.coreIslands.Count;
        int normal = 0;
        foreach (var item in IslandManager.Instance.m_islandSet.normalIslands)
        {
            if (item.isActive())
            {
                normal++;
            }
        }
        int all = coreIsland + normal;
        if (all >= 8)
        {
            GameEvents.Sigton.GetEvent<Subject<SubjectArg>>("onHappyEnd")
            .OnNext(new SubjectArg("msg from legislator,happy end"));
        }
        else
        {
            GameEvents.Sigton.GetEvent<Subject<SubjectArg>>("onBadEnd")
            .OnNext(new SubjectArg("msg from legislator,bad end"));
        }
    }
    void Start()
    {
        GameEvents.Sigton.RegisterEvent(EventDictionaryType.MechanismEvent, "onHappyEnd");
        GameEvents.Sigton.RegisterEvent(EventDictionaryType.MechanismEvent, "onBadEnd");

        WatchGameEnd();
    }
}
