using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public class PlayerInteractPresenter : MonoBehaviour
{
    PlayerStateModel stateModel;
    SimplePlayerInventoryPresenter inventory;

    Dictionary<string, Action> playerInteractBehavior = new Dictionary<string, Action>();

    private void Init()
    {
        stateModel = GetComponent<PlayerStatePresenter>().StateModel;
        inventory = GetComponent<SimplePlayerInventoryPresenter>();
        playerInteractBehavior.Add("NPC", () =>
        {
            print("start dialog");
        });
    }

    private void Start()
    {
        Init();

        stateModel.playerState
            .Where(x=>stateModel.playerState.Value == PlayerState.InteractState)
            .Subscribe(x =>
            {
                print("interact action start");
            });

        GameEvents.Sigton.onInteractStart += (IInteractable type) =>
        {
            //print(type.InteractObjectType);
        };

        GameEvents.Sigton.onInteractEnd += () =>
        {
            stateModel.playerState.Value = PlayerState.MotionState;
        };

        GameEvents.Sigton.onResourceCollected += (string type, int account) =>
        {
            if (type == "FoodMaterial")
            {
               
                inventory.FoodMaterial.Value += account;
                print("player get food " + inventory.FoodMaterial.Value);
            }
            
        };

        GameEvents.Sigton.onIslandCreated += () =>
        {
            inventory.BuildingMaterial.Value -= 15;
            print("player use building material " + 15);

        };
    }
}
