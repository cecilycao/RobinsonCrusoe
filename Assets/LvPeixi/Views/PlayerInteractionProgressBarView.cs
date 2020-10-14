using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class PlayerInteractionProgressBarView : MonoBehaviour
{
    private Slider slider;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        slider.gameObject.SetActive(false);

        GUIEvents.Singleton.InteractionProgressBar
            .Subscribe(TickInteractionProgressBar);

    }
    void TickInteractionProgressBar(float time)
    {
        slider.gameObject.SetActive(true);
        IDisposable progressMircotine = null;
        float _progress = time;
        Vector3 _playerPosInWorld = GameObject.Find("PlayerHandle").transform.position;
        Vector3 _playerPosInScreen = Camera.main.WorldToScreenPoint(_playerPosInWorld);

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = _playerPosInScreen + Vector3.up * 100;

        progressMircotine = Observable.EveryUpdate()
            .Subscribe(x =>
            {
                _progress -= Time.deltaTime;
                _progress = Mathf.Clamp(_progress, 0, time);
                slider.value = _progress;
                if (_progress<=0)
                {
                    //print("key released,close progress bar");
                    slider.gameObject.SetActive(false);
                    progressMircotine.Dispose();
                }
            });       
    }
}
