using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class PlayerBehaviorView : MonoBehaviour
{
    Rigidbody rigid;
    PlayerMovementPresenter movement;
    Animator anim;

    [SerializeField]
    Vector3 textVelocity;

    private void Awake()
    {
        movement = GetComponent<PlayerMovementPresenter>();
        anim = GetComponentInChildren<Animator>();
        rigid = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        Observable.EveryFixedUpdate()
            .Subscribe(x =>
            {
                rigid.velocity = -movement.Velocity.Value;
            });

        Observable.EveryUpdate()
            .Subscribe(x =>
            {
                anim.SetFloat("horizontal", FilterVelocityData(movement.Velocity.Value.x));
                anim.SetFloat("vertical", FilterVelocityData(movement.Velocity.Value.z));
                textVelocity = movement.Velocity.Value;
            });
    }
    float FilterVelocityData(float axisSpeed)
    {
        if (axisSpeed>0)
        {
            return 1;
        }
        else if (axisSpeed<0)
        {
            return -1;
        }
        return 0;
    }
}
