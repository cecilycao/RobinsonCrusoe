using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;
using Peixi;
public class BuildSystemPlayTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        IIslandGridModule grid = GetComponent<IIslandGridModule>();

        var timer = Observable.Interval(TimeSpan.FromSeconds(1));

        timer
            .Where(x => x < 3)
            .Subscribe(y =>
            {
                var pos = new Vector2Int((int)y, 0);
                grid.BuildIslandAt(pos);
            });
        
        timer
            .Where(x => x == 4)
            .Subscribe(y =>
            {
                grid.RemoveIslandAt(new Vector2Int(1, 0));
            });

        timer
            .Where(x => x == 6)
            .Subscribe(y =>
            {

                grid.BuildIslandAt(new Vector2Int(1, 0));
            });
      
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
