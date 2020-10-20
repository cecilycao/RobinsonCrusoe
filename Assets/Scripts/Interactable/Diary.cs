using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Diary : MonoBehaviour, IInteractable
{
    public GameObject Icon;
    bool canWriteDiary = false;
    bool diaryOpen = false;
    public int maxFatigue = 100;
    public string InteractObjectType => "Diary";
    public Vector3 IconOffset = new Vector3(0, 7, 0);

    // Start is called before the first frame update
    void Start()
    {

        Icon = FindObjectOfType<IconManager>().DiaryIcon;
        if (Icon == null)
        {
            Debug.LogError("Icon haven't been assigned to IconManager");
        }

        var config = GameConfig.Singleton.InteractionConfig;
        
        GUIEvents.Singleton.Fatigue
            .Where(y => y == maxFatigue)
            .Subscribe(x =>
            {
                Debug.Log("Can Write Diary now.");
                canWriteDiary = true;
            });
    }

    private void Update()
    {
        Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
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
        Icon.SetActive(true);
    }

    public void HideIcon()
    {
        Icon.SetActive(false);
    }

}
