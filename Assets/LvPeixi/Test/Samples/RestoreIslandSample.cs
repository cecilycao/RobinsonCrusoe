using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreIslandSample : MonoBehaviour,IInteractableIsland
{
    string islandType = "BrokenIsland";
    string materialType = "BuildingMaterial";
    int cost = 10;
    public virtual string MaterialType => materialType;
    public virtual int MaterialCost => cost;
    public virtual string InteractObjectType => islandType;

    public virtual void EndContact()
    {
        Mediator.Sigton.EndInteract();
    }
    public virtual void StartContact()
    {
        Mediator.Sigton.StartInteraction(this);
    }

    public virtual void StartInteract()
    {
        
    }

    public virtual void EndInteract(object result)
    {
       
    }
}
