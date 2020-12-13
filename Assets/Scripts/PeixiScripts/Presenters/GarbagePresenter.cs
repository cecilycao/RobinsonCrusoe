using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GarbagePresenter : MonoBehaviour
{
    public void InitGarbage(float speed,Vector3 moveDirection)
    {
        Observable.EveryUpdate()
            .Subscribe(x =>
            {
                Destroy(gameObject, 10);
                transform.position += speed * moveDirection.normalized * Time.deltaTime;             
            }).AddTo(this);
    }
}
