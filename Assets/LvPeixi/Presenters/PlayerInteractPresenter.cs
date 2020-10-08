using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public class PlayerInteractPresenter : MonoBehaviour,IPlayerInteractPresenter
{
    #region//private variables
    PlayerStateModel stateModel;
    SimplePlayerInventoryPresenter inventory;
    Dictionary<PlayerInteractionType, Action> playerInteractBehavior = new Dictionary<PlayerInteractionType, Action>();
    #endregion

    #region//player interaction behaviors
    public void PlayerEndInteraction()
    {
        stateModel.playerState.Value = PlayerState.MotionState;
    }
    public void PlayerStartInteraction(PlayerInteractionType interact)
    {
        stateModel.playerState.Value = PlayerState.InteractState;
        playerInteractBehavior[interact]();
    }
    void PlayerCollectBehavior() { }
    void PlayerDialogBehavior() { }
    #endregion

    private void Init()
    {
        stateModel = GetComponent<PlayerStatePresenter>().StateModel;
        inventory = GetComponent<SimplePlayerInventoryPresenter>();
        Mediator.Sigton.PlayerInteract = this;

        playerInteractBehavior[PlayerInteractionType.Collect] = PlayerCollectBehavior;
        playerInteractBehavior[PlayerInteractionType.Dialog] = PlayerDialogBehavior;
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
            if (type == "BuildingMaterial")
            {

                inventory.BuildingMaterial.Value += account;
                print("player get building material " + inventory.BuildingMaterial.Value);
            }

        };

        GameEvents.Sigton.onIslandCreated += () =>
        {
            inventory.BuildingMaterial.Value -= 15;
            print("player use building material " + 15);

        };
    }
}
public enum PlayerInteractionType
{
    Dialog,
    Collect
}
