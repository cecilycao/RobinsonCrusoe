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
    public Vector3 IconOffset = new Vector3(0, 7, 0);

    private void Start()
    {
        //Icon = FindObjectOfType<IconManager>().NPCTalkIcon;
        //if (Icon == null)
        //{
        //    Debug.LogError("Icon haven't been assigned to IconManager");
        //}
    }

    private void Update()
    {
        //Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
    }

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
        Icon.SetActive(true);
    }

    public void HideIcon()
    {
        Icon.SetActive(false);
    }

}
