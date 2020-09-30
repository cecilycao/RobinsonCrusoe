using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class TimeSystemView : MonoBehaviour
{
    public Text time;
    public Text day;
    // Start is called before the first frame update
    void Start()
    {
        GameEvents.Sigton.timeSystem
            .Subscribe(_data =>
            {
                time.text = _data.TimeCountdown.ToString();
                day.text = _data.DayCount.ToString();
            });

        GameEvents.Sigton.onDayStart += () =>
        {
            //print("day start");
        };
        GameEvents.Sigton.onDayEnd += () =>
        {
            //print("day end");
        };
    }
}
