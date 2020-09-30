using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerStateModel 
{
    public ReactiveProperty<PlayerState> playerState = new ReactiveProperty<PlayerState>(PlayerState.MotionState);
}
public enum PlayerState
{
    MotionState,
    InteractState
}
