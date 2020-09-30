using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerBehaviorView : MonoBehaviour
{
    Rigidbody rigid;
    PlayerMovementPresenter movement;

    private void Awake()
    {
        movement = GetComponent<PlayerMovementPresenter>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Observable.EveryFixedUpdate()
            .Subscribe(x =>
            {
                rigid.velocity = movement.Velocity.Value;
            });
    }
}
