using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Diary : MonoBehaviour, IInteractable
{
    bool canWriteDiary = false;
    bool diaryOpen = false;
    public int maxFatigue = 100;
    public string InteractObjectType => "Diary";

    // Start is called before the first frame update
    void Start()
    {
        var config = GameConfig.Singleton.InteractionConfig;
        GUIEvents.Singleton.Fatigue
            .Where(y => y == maxFatigue)
            .Subscribe(x =>
            {
                Debug.Log("Can Write Diary now.");
                canWriteDiary = true;
            });
    }


    public void OnDiaryOpen()
    {
        if (canWriteDiary)
        {
            
            DiaryManager.Instance.createNewPage();
            canWriteDiary = false;
            diaryOpen = true;
        }
    }

    public void OnDiaryClose()
    {
        if (diaryOpen)
        {
            DiaryManager.Instance.closeDiary();
            Debug.Log("Finish writing diary, one day end");
            GameEvents.Sigton.onDayEnd.Invoke();
            diaryOpen = false;
        }
    }

    public void StartContact()
    {
        Mediator.Sigton.OpenDiary(this);
    }

    public void EndContact()
    {
        OnDiaryClose();
        
    }

    public void StartInteract()
    {
    }

    public void EndInteract(object result)
    {
    }

    public void ShowIcon()
    {
        //Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        //Icon.SetActive(true);
    }

    public void HideIcon()
    {
        //Icon.SetActive(false);
    }

}
