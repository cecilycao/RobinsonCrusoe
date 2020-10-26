using UnityEngine;
using UnityEngine.UI;
using UniRx;
using System;

public class PlayerInteractionProgressBarView : MonoBehaviour
{
    private Slider slider;
    
    [Header("-----TEST VARIABLE BLOCK------")]

    public int runOperTime = 0;

    public int closeOperTime = 0;

    public int playCancelTimes = 0;

    public bool isActive;

    IDisposable progressMircotine;
    // Start is called before the first frame update
    void Start()
    {
        slider = GetComponentInChildren<Slider>();
        slider.gameObject.SetActive(false);

        GUIEvents.Singleton.InteractionProgressBar
            .Where(x => !isActive)
            .Subscribe(TickInteractionProgressBar);

        OnPlayerCancelBuilding();
    }
    void OnPlayerCancelBuilding()
    {
        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.onInteractBtnReleased)
            .Subscribe(x =>
            {
                OnProgressBarCompleted();
            });
    }
    void TickInteractionProgressBar(float time)
    {
        runOperTime++;
        isActive = true;
        slider.gameObject.SetActive(true);
 
        float _progress = 0;
        Vector3 _playerPosInWorld = GameObject.Find("PlayerHandle").transform.position;
        Vector3 _playerPosInScreen = Camera.main.WorldToScreenPoint(_playerPosInWorld);

        RectTransform rectTransform = GetComponent<RectTransform>();
        rectTransform.position = _playerPosInScreen + Vector3.up * 100;

        progressMircotine = Observable.EveryUpdate()
            .Subscribe(x =>
            {
                _progress += Time.deltaTime / time;
                _progress = Mathf.Clamp(_progress, 0, time);
                slider.value = _progress;
                if (_progress>=1)
                {
                    OnProgressBarCompleted();
                }
            });       
    }
    void OnProgressBarCompleted()
    {
        closeOperTime++;
        slider.gameObject.SetActive(false);
        isActive = false;
        if (progressMircotine != null)
        {
            progressMircotine.Dispose();
        }  
    }
}
