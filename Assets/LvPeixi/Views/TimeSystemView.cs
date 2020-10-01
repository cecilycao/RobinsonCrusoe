using UnityEngine.UI;
using UnityEngine;
using System;
using UniRx;

public class TimeSystemView : MonoBehaviour
{
    public Text time;
    public Text day;
    public RawImage blackScreen;
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
            BlackScreenFadeOut();
        };
        GameEvents.Sigton.onDayEnd += () =>
        {
            BlackScreenFadeIn();
        };
    }

    void BlackScreenFadeOut()
    {
        IDisposable fadeInMircotine = null;
        Color _color = new Color();
        fadeInMircotine = Observable.EveryLateUpdate()
            .Subscribe(x => 
            {
                _color.a = Mathf.Lerp(blackScreen.color.a, 0, 0.1f);
                blackScreen.color = _color;
                if (_color.a < 0.05f)
                {
                    _color.a = 0;
                    blackScreen.color = _color;
                    fadeInMircotine.Dispose();
                }
            });
    }

    void BlackScreenFadeIn()
    {
        IDisposable fadeInMircotine = null;
        Color _color = new Color();
        fadeInMircotine = Observable.EveryLateUpdate()
        .Subscribe(x =>
        {
            _color.a = Mathf.Lerp(blackScreen.color.a,1, 0.1f);
            blackScreen.color = _color;
            if (_color.a > 0.99f)
            {
                _color.a = 1;
                blackScreen.color = _color;
                fadeInMircotine.Dispose();
            }
        });
    }
}  
