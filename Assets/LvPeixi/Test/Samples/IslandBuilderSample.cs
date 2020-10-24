using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class IslandBuilderSample : MonoBehaviour,IIslandBuilder
{
    [SerializeField]
    private int materialCost = 15;
    public GameObject Icon;
    [SerializeField]
    private string interactObjectType = "BuildMaterial";
    public int MaterialCost => materialCost;
    public string InteractObjectType => interactObjectType;
    public Vector3 IconOffset = new Vector3(0, 7, 0);
    bool isSick = false;

    private void Start()
    {
        //Icon = FindObjectOfType<IconManager>().BuildIslandIcon;
        //if (Icon == null)
        //{
        //    Debug.LogError("Icon haven't been assigned to IconManager");
        //}
        GameEvents.Sigton.onNPCSicked
         .Subscribe(x =>
         {
             isSick = true;
         });
        GameEvents.Sigton.onNPCSickedEnd
             .Subscribe(x =>
             {
                 isSick = false;
             });
        GameEvents.Sigton.onPlayerSicked
             .Subscribe(x =>
             {
                 isSick = true;
             });
        GameEvents.Sigton.onPlayerSickedEnd
             .Subscribe(x =>
             {
                 isSick = false;
             });
    }

    private void Update()
    {
        //Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
    }

    public void EndContact()
    {
        if (!isSick)
        {
            //Mediator.Sigton.EndInteract();
        }
    }

    public void EndInteract(object result)
    {
        GameEvents.Sigton.onIslandCreated.Invoke();
        IslandManager.Instance.createIsland();
        print("island build end interact with player, start build a new island");
    }

    public void StartContact()
    {
        if (!isSick)
        {
            Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
            Mediator.Sigton.StartInteraction(this);
        }
    }

    public void StartInteract()
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
