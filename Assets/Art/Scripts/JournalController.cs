using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalController : MonoBehaviour
{
    public Animator journalAnim;
    public GameObject buttons;
    public GameObject journalIcon;
    
    
    public void JournalOpen()
    {
        buttons.SetActive(false);
        journalAnim.SetBool("OPEN",true);
        Invoke("ButtonsShow",0.8f);
    }
    private void ButtonsShow()
    {
        buttons.SetActive(true);
    }

    public void JournalClose()
    {
        buttons.SetActive(false);
        journalAnim.SetBool("OPEN",false);
        Invoke("JournalHide",0.8f);
    }
    private void JournalHide()
    {
        journalAnim.gameObject.SetActive(false);
        journalIcon.gameObject.SetActive(true);
    }
}
