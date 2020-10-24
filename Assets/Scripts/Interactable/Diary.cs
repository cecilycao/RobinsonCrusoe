﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Diary : MonoBehaviour, IInteractable
{
    public GameObject Icon;

    public bool canWriteDiary = false;
    bool diaryOpen = false;
    //int maxFatigue;
    public string InteractObjectType => "Diary";
    public Vector3 IconOffset = new Vector3(0, 7, 0);
    bool isSick = false;

    // Start is called before the first frame update
    void Start()
    {
        Icon = FindObjectOfType<IconManager>().DiaryIcon;
        if (Icon == null)
        {
            Debug.LogError("Icon haven't been assigned to IconManager");
        }

        GameEvents.Sigton.onNPCSicked
         .Subscribe(x =>
         {
             isSick = true;
         });
        GameEvents.Sigton.onNPCSickedEnd
         .Subscribe(x =>
         {
             isSick = false;
         });
        GameEvents.Sigton.onPlayerSicked
         .Subscribe(x =>
         {
             isSick = true;
         });
        GameEvents.Sigton.onPlayerSickedEnd
         .Subscribe(x =>
         {
             isSick = false;
         });

        GameEvents.Sigton.onFatigueReachMax
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

            DiaryManager.Instance.createNewPage();
            canWriteDiary = false;
            diaryOpen = true;

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
        if (canWriteDiary && !isSick)
        {

            Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
            Mediator.Sigton.OpenDiary(this);
        }
    }

    public void EndContact()
    {
        if (!isSick)
        {
            Mediator.Sigton.EndInteract();
        }
        
        
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
