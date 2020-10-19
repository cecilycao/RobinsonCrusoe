using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class FXManager : MonoBehaviour
{
    public float startIntensity;
    public float endIntensity;
    public Vector3 startRotation;
    public Vector3 endRotation;

    public Vector3 currentRot;

    public Light dirLight;
    // Start is called before the first frame update
    void Start()
    {
        dirLight.intensity = startIntensity;
        //dirLight.transform.rotation = Quaternion.Euler(startRotation);
        currentRot = startRotation;
        Observable.EveryUpdate()
             .Subscribe(x =>
             {
                 dirLight.intensity = Mathf.MoveTowards(dirLight.intensity, endIntensity, (endIntensity - startIntensity) / 20 * Time.deltaTime);

                 float _rotSpeed_x = (endRotation.x - startRotation.x) / 20 * Time.deltaTime;
                 _rotSpeed_x = Mathf.Abs(_rotSpeed_x);
                 float _rotSpeed_y = (endRotation.y - startRotation.y) / 20 * Time.deltaTime;
                 _rotSpeed_y = Mathf.Abs(_rotSpeed_y);
                 float _rotSpeed_z = (endRotation.z - startRotation.z) / 20 * Time.deltaTime;
                 _rotSpeed_z = Mathf.Abs(_rotSpeed_z); 
                 currentRot.x = Mathf.MoveTowards(currentRot.x, endRotation.x, _rotSpeed_x);
                 currentRot.y = Mathf.MoveTowards(currentRot.y, endRotation.y, _rotSpeed_y);
                 currentRot.z = Mathf.MoveTowards(currentRot.z, endRotation.z, _rotSpeed_z);

                 dirLight.transform.rotation = Quaternion.Euler(currentRot);
             });
    }
}
