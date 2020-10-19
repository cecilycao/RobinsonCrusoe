using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IslandBuilderSample : MonoBehaviour,IIslandBuilder
{
    [SerializeField]
    private int materialCost = 15;
    public GameObject Icon;
    [SerializeField]
    private string interactObjectType = "BuildMaterial";
    public int MaterialCost => materialCost;
    public string InteractObjectType => interactObjectType;

    public void EndContact()
    {
        
        //Mediator.Sigton.EndInteract();
    }

    public void EndInteract(object result)
    {
        GameEvents.Sigton.onIslandCreated.Invoke();
        IslandManager.Instance.createIsland();
        print("island build end interact with player, start build a new island");
    }

    public void StartContact()
    {
        Mediator.Sigton.StartInteraction(this);
    }

    public void StartInteract()
    {
    }

    public void ShowIcon()
    {
        Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Icon.SetActive(true);
    }

    public void HideIcon()
    {
        Icon.SetActive(false);
    }

}
