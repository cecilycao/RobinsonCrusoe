using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SickUIManager : MonoBehaviour
{
    public float duration;
    public GameObject PlayerSickUI;
    public GameObject NPCSickUI;
    public TimeSystemView view;

    // Start is called before the first frame update
    void Start()
    {
        view = FindObjectOfType<TimeSystemView>();
        PlayerSickUI.SetActive(false);
        NPCSickUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void showNPCSickUI()
    {
        view.BlackScreenFadeIn();
        
        StartCoroutine(ShowNPCSickUICouroutine());
    }

    IEnumerator ShowNPCSickUICouroutine()
    {
        yield return new WaitForSeconds(1);
        NPCSickUI.SetActive(true);
        //yield return new WaitForSeconds(duration);
        
    }

    void finishShowNPCSickUI()
    {
        NPCSickUI.SetActive(false);
        StartCoroutine(finishShowNPCSickUICouroutine());
    }

    IEnumerator finishShowNPCSickUICouroutine()
    {
        yield return new WaitForSeconds(1);
        view.BlackScreenFadeOut();
        yield return new WaitForSeconds(1);
    }

    void showPlayerSickUI()
    {
        view.BlackScreenFadeIn();
        PlayerSickUI.SetActive(true);
        StartCoroutine(finishShowPlayerSickUI());
    }


    IEnumerator finishShowPlayerSickUI()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(duration);
        PlayerSickUI.SetActive(false);
        DiaryManager.Instance.createNewPage();
        yield return new WaitForSeconds(3);
        //todo: do it on diraymanager maybe
        GameEvents.Sigton.onDayEnd.Invoke();
        
        //yield return new WaitForSeconds(1);
        
    }

    void showPlayerSickUIWithHelp()
    {
        view.BlackScreenFadeIn();
        PlayerSickUI.SetActive(true);

        //StartCoroutine(finishShowPlayerSickUINoHelp());
    }

    void finishShowPlayerSickWithHelp()
    {
        StartCoroutine(finishShowPlayerSickUIWithHelpHelper());
    }
    IEnumerator finishShowPlayerSickUIWithHelpHelper()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(duration);
        PlayerSickUI.SetActive(false);
        DiaryManager.Instance.createNewPage();
        yield return new WaitForSeconds(3);
        //todo: do it on diraymanager maybe
        GameEvents.Sigton.onDayEnd.Invoke();

    }
}
