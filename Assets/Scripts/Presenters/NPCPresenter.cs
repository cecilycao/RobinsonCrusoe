using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NPCPresenter : MonoBehaviour,IInteractableNPC
{
    [SerializeField]
    NPCModel npcModel = new NPCModel();
    public string InteractObjectType { get => npcModel.interactObjectType; }
    public string NPCName { get => npcModel.npcName; }
    public void StartInteractWithPlayer()
    {
        Mediator.Sigton.StartDialog(this);
    }
    public void EndInteractWithPlayer()
    {
        Mediator.Sigton.EndInteract();
    }
    public void OnDialogStart()
    {
      
    }
    public void OnDialogEnd()
    {
        print("dialog end and Alice exploded");
    }
}
