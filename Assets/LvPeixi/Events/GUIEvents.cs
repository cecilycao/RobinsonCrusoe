using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class GUIEvents:MonoBehaviour,IGUIEvents
{
    static IGUIEvents _sig;
    #region//The register-needed events
    ReactiveProperty<int> fatigue;
    ReactiveProperty<int> foodMaterial;
    ReactiveProperty<int> buildingMaterial;
    ReactiveProperty<int> hunger;
    #endregion
    Subject<string> broadcastInteractTipMessage = new Subject<string>();
    Subject<float> interactionProgressBar = new Subject<float>();
    Subject<bool> playerStartFishing = new Subject<bool>();
    Subject<bool> playerEndFishing = new Subject<bool>();
    public static IGUIEvents Singleton
    {
        get => _sig;
        set
        {
            if (_sig== null)
            {
                _sig = value;
            }
        }
    }
    public ReactiveProperty<int> Fatigue
    {
        get => fatigue;
        set
        {
            if (fatigue == null)
            {
                fatigue = value;
            }
        }
    }
    public ReactiveProperty<int> FoodMaterial
    {
        get => foodMaterial;
        set
        {
            if (foodMaterial == null)
            {
                foodMaterial = value;
            }
        }
    }
    public ReactiveProperty<int> BuildingMaterial
    {
        get => buildingMaterial;
        set
        {
            if (buildingMaterial==null)
            {
                buildingMaterial = value;
            }
        }
    }
    public ReactiveProperty<int> Hunger
    {
        get => hunger;
        set
        {
            if (hunger == null)
            {
                hunger = value;
            }
        }
    }
    public Subject<string> BroadcastInteractTipMessage
    {
        get => broadcastInteractTipMessage;
    }
    public Subject<float> InteractionProgressBar { get => interactionProgressBar; }
    public Subject<bool> PlayerStartFishing { get => playerStartFishing; }
    public Subject<bool> PlayerEndFishing { get => playerEndFishing; }
    private void Awake()
    {
        _sig = this;
    }

}
