using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlotProcedure : MonoBehaviour
{
    public int NPCSickedDay = 7;
    public int PlayerSickedDay = 13;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == NPCSickedDay && x.IsDay)
            .First()
            .Subscribe(x =>
            {
                GameEvents.Sigton.onNPCSicked.OnNext(NPCSickedDay);
            });

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == PlayerSickedDay && x.IsDay)
            .First()
            .Subscribe(x =>
            {
                GameEvents.Sigton.onPlayerSicked.OnNext(PlayerSickedDay);
            });

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == PlayerSickedDay + 1 && x.IsDay)
            .First()
            .Subscribe(x =>
            {
                GameEvents.Sigton.onPlayerSickedEnd.OnNext(PlayerSickedDay + 1);
            });
    }

    public void NPCSickEnd()
    {
        Debug.Log("NPC Sick End");
        GameEvents.Sigton.onNPCSickedEnd.OnNext(NPCSickedDay + 1);
    }
}


