using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreIslandSample : MonoBehaviour,IInteractableIsland
{
    public string MaterialType => materialType;
    public int MaterialCost => cost;
    public string InteractObjectType => islandType;
    public void EndInteractWithPlayer()
    {
        Mediator.Sigton.EndInteract();
    }
    public void OnIslandRestoreEnd()
    {
        print("play island restore animation");
    }
    public void OnIslandRestoreStart()
    {
        
    }
    public void StartInteractWithPlayer()
    {
        Mediator.Sigton.StartRestoreIsland(this);
    }

    string islandType = "BrokenIsland";
    string materialType = "BuildingMaterial";
    int cost = 10;
}
