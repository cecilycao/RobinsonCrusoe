using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodMaker : MonoBehaviour, IInteractable
{
    public string interactObjectType = "FoorMaker";
    public string InteractObjectType { get => interactObjectType; }

    public void OnFoodMakeEnd()
    {
        //show anim?
    }

    public void OnFoodMakeBegin()
    {

    }
    public void StartContact()
    {
        if (true)
        {
            //向Mediator通知要进行的互动行为
            //Mediator.Sigton.StartProcessFood(this);
        }
    }
    public void EndContact()
    {
        //Mediator.Sigton.EndInteract();
    }
    public void StartInteract()
    {
        throw new System.NotImplementedException();
    }
    public void EndInteract(object result)
    {
        throw new System.NotImplementedException();
    }
}
