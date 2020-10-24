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
        
        StartCoroutine(finishShowNPCSickUI());
    }

    IEnumerator finishShowNPCSickUI()
    {
        yield return new WaitForSeconds(1);
        NPCSickUI.SetActive(true);
        yield return new WaitForSeconds(duration);
        NPCSickUI.SetActive(false);
        view.BlackScreenFadeOut();
        yield return new WaitForSeconds(1);
    }

    void showPlayerSickUI()
    {
        PlayerSickUI.SetActive(true);
        StartCoroutine(finishShowPlayerSickUI());
    }

    IEnumerator finishShowPlayerSickUI()
    {
        yield return new WaitForSeconds(1);
        yield return new WaitForSeconds(duration);
        PlayerSickUI.SetActive(false);
        view.BlackScreenFadeOut();
        yield return new WaitForSeconds(1);
    }

}
