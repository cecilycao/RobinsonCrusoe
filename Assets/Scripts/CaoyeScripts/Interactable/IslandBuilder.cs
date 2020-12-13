using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandBuilder : MonoBehaviour, IInteractable
{
    public string interactObjectType = "IslandBuilder";
    public int MaterialCost = 15;
    public string InteractObjectType { get => interactObjectType; }

    public void OnIslandBuildEnd()
    {
        IslandManager.Instance.createIsland();
        print("IslandManager creat island");
    }

    public void OnIslandBuild()
    {
        
    }

    public void StartContact()
    {
        if (true)
        {
            //向Mediator通知要进行的互动行为
            //Mediator.Sigton.StartAddIsland(this);
        }
    }

    public void EndContact()
    {
        Mediator.Sigton.EndInteract();
    }

    public void StartInteract()
    {
        throw new System.NotImplementedException();
    }

    public void EndInteract(object result)
    {
        throw new System.NotImplementedException();
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
