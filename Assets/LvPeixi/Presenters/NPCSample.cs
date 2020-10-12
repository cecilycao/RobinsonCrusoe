using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NPCSample : MonoBehaviour,IInteractableNPC
{
    [SerializeField]
    NPCModel npcModel = new NPCModel();
    public string InteractObjectType { get => npcModel.interactObjectType; }
    public string NPCName { get => npcModel.npcName; }
    public void StartContact()
    {
        Mediator.Sigton.StartInteraction(this);
    }
    public void EndContact()
    {
        Mediator.Sigton.EndInteract();
    }
    public void StartInteract()
    {
        
    }
    public void EndInteract(object result)
    {
        
    }
}
