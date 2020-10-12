using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreIslandSample : MonoBehaviour,IInteractableIsland
{
    string islandType = "BrokenIsland";
    string materialType = "BuildingMaterial";
    int cost = 10;
    public string MaterialType => materialType;
    public int MaterialCost => cost;
    public string InteractObjectType => islandType;
    public void EndContact()
    {
        Mediator.Sigton.EndInteract();
    }
    public void StartContact()
    {
        Mediator.Sigton.StartInteraction(this);
    }

    public void StartInteract()
    {
        
    }

    public void EndInteract(object result)
    {
        print("play island restore animation");
    }

   
}
