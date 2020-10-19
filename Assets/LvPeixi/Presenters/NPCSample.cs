using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NPCSample : MonoBehaviour,IInteractableNPC
{
    [SerializeField]
    NPCModel npcModel = new NPCModel();
    public GameObject Icon;
    public string InteractObjectType { get => npcModel.interactObjectType; }
    public string NPCName { get => npcModel.npcName; }
    public int preference = 0;
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
        preference += 5;
        //todo： 减玩家疲劳值 -10
        var attr = FindObjectOfType<PlayerAttributePresenter>();
        attr.Fatigue.Value -= 10;
    }
    public void EndInteract(object result)
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
