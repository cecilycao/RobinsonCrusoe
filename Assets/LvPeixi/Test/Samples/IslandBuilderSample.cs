using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandBuilderSample : MonoBehaviour,IIslandBuilder
{
    [SerializeField]
    private int materialCost = 15;
    [SerializeField]
    private string interactObjectType = "BuildMaterial";
    public int MaterialCost => materialCost;
    public string InteractObjectType => interactObjectType;

    public void EndContact()
    {
        Mediator.Sigton.EndInteract();
    }

    public void EndInteract(object result)
    {
        print("island build end interact with player, start build a new island");
    }

    public void StartContact()
    {
        Mediator.Sigton.StartInteraction(this);
    }

    public void StartInteract()
    {
        
    }
}
