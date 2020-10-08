using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandBuilder : MonoBehaviour, IInteractable
{
    public string interactObjectType = "IslandBuilder";
    public int MaterialCost = 30;
    public string InteractObjectType { get => interactObjectType; }

    public void OnIslandBuildEnd()
    {
        IslandManager.Instance.createIsland();
    }

    public void OnIslandBuild()
    {
        
    }

    public void StartInteractWithPlayer()
    {
        if (true)
        {
            //向Mediator通知要进行的互动行为
            Mediator.Sigton.StartAddIsland(this);
        }
    }

    public void EndInteractWithPlayer()
    {
        Mediator.Sigton.EndInteract();
    }
}
