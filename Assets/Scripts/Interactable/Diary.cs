using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class Diary : MonoBehaviour, IInteractable
{

    public string InteractObjectType => "Diary";


    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void OnDiaryOpen()
    {
        //todo: can only do this(write diary) when day end
        DiaryManager.Instance.createNewPage();

    }

    public void OnDiaryClose()
    {
        DiaryManager.Instance.closeDiary();
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
}
