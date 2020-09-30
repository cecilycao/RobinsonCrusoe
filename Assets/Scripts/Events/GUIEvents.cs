using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

public class GUIEvents:MonoBehaviour
{
    static GUIEvents _sig;
    ReactiveProperty<int> vitality = new ReactiveProperty<int>();
    ReactiveProperty<int> foodMaterial = new ReactiveProperty<int>();
    ReactiveProperty<int> buildingMaterial = new ReactiveProperty<int>();
    ReactiveProperty<int> hunger = new ReactiveProperty<int>();
    Subject<string> broadcastInteractTipMessage = new Subject<string>();

    public ReactiveProperty<int> Vitality
    {
        get => vitality;
        set
        {
            if (vitality == null)
            {
                vitality = value;
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
    public static GUIEvents Singleton
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
    private void Awake()
    {
        _sig = this;
    }

}
