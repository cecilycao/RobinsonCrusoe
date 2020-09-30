using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerStatePresenter : MonoBehaviour
{
    PlayerStateModel stateModel = new PlayerStateModel();
    public PlayerStateModel StateModel
    {
        get => stateModel;
    }
    [Header("the readonly variables")]
    [SerializeField]
    PlayerState state;
    private void Awake()
    {
        stateModel.playerState.Value = PlayerState.MotionState;
    }
    // Start is called before the first frame update
    void Start()
    {
        ForTest();
    }
    void ForTest()
    {
        stateModel.playerState
            .Subscribe(x =>
            {
                state = x;
            });
    }
}
