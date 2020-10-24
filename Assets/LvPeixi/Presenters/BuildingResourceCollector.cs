﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class BuildingResourceCollector : MonoBehaviour,IBuildingResourceCollector
{
    public GameObject Icon;
    public int resourceAccount = 15;
    public string resourceType = "BuildingMaterial";
    public string interactObjectType = "ResourceCollector";
    public int ResourceAccount { get => resourceAccount; }
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
            resourceAccount = 15;
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
            resourceAccount = 0;
        }
    }

    public void StartInteract()
    {

    }

    public void StartContact()
    {
        if (resourceAccount > 0 && !isSick)
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