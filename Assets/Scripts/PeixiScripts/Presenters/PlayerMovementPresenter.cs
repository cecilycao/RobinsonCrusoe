using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerMovementPresenter : MonoBehaviour
{
    PlayerMovementModel movementModel = new PlayerMovementModel();
    PlayerStateModel stateModel = new PlayerStateModel();
    public float moveSpeed = 5;
    public ReactiveProperty<Vector3> Velocity
    {
        get => movementModel.velocity;
    }

    [Header("=====Test data=====")]
    [SerializeField]
    Vector3 velocity;
    private void Awake()
    {
        stateModel = GetComponent<PlayerStatePresenter>().StateModel;
    }
    private void Start()
    {
        stateModel.playerState
            .Where(x => stateModel.playerState.Value == PlayerState.MotionState)
            .Subscribe(x =>
            {

            });

        Observable.EveryFixedUpdate()
            .Where(x=>stateModel.playerState.Value == PlayerState.MotionState)
            .Subscribe(x =>
            {
                Vector3 _direction = new Vector3(Input.GetAxis("Horizontal"),0, Input.GetAxis("Vertical"));
                _direction = -_direction.normalized; ;
                Velocity.Value = _direction * moveSpeed;
                velocity = Velocity.Value;
            });

        #region//InteractState
        stateModel.playerState
        .Where(x => stateModel.playerState.Value == PlayerState.InteractState)
        .Subscribe(x =>
        {
            movementModel.velocity.Value = Vector3.zero;
        });
        #endregion
    }
}
