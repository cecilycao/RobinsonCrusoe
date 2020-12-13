using UnityEngine;
using UniRx;

public class PlayerMovementModel
{
    public ReactiveProperty<Vector3> velocity = new ReactiveProperty<Vector3>(Vector3.zero);
}
