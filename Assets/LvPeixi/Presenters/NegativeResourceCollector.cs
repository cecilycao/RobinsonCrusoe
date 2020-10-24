using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class NegativeResourceCollector : MonoBehaviour,INegativeResourceCollector
{
    public GameObject Icon;
    public int resourceAccount_buildingMaterial = 15;
    public int resourceAccount_foodMaterial = 5;
    public string resourceType = "BuildingMaterial";
    public string interactObjectType = "ResourceCollector";
    public int ResourceAccount_buildingMat { get => resourceAccount_buildingMaterial; }
    public int ResourceAccount_foodMat => resourceAccount_foodMaterial;
    public string ResourceType { get => resourceType; }
    public string InteractObjectType { get => interactObjectType; }
    public Vector3 IconOffset = new Vector3(0, 7, 0);
    bool isSick = false;

    private void Start()
    {
        Icon = FindObjectOfType<IconManager>().CollectGarbageIcon;
        if (Icon == null)
        {
            Debug.LogError("Icon haven't been assigned to IconManager");
        }
        GameEvents.Sigton.onDayStart += () =>
        {
            resourceAccount_buildingMaterial = 15;
        };
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
        Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
    }

    public void EndContact()
    {
        if (!isSick)
        {
            Mediator.Sigton.EndInteract();
        }
    }

    public void EndInteract(object result)
    {
        bool _res = (bool)result;
        if (_res)
        {
            resourceAccount_buildingMaterial = 0;
        }
    }

    public void StartInteract()
    {

    }

    public void StartContact()
    {
        if (resourceAccount_buildingMaterial > 0 && !isSick)
        {
            //向Mediator通知要进行的互动行为
            Mediator.Sigton.StartInteraction(this);
        }
        if (resourceAccount_buildingMaterial > 0)
        {
            //向Mediator通知要进行的互动行为
            Mediator.Sigton.StartInteraction(this);
        }
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
