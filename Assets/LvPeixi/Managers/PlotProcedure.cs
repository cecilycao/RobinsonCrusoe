using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlotProcedure : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == 7 && x.IsDay)
            .First()
            .Subscribe(x =>
            {
                GameEvents.Sigton.onNPCSicked.OnNext(7);
            });

        GameEvents.Sigton.timeSystem
            .Where(x => x.DayCount == 13 && x.IsDay)
            .First()
            .Subscribe(x =>
            {
                GameEvents.Sigton.onPlayerSicked.OnNext(13);
            });
    }
}
