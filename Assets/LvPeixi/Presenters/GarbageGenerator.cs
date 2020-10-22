using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UnityEditor;

public class GarbageGenerator : MonoBehaviour
{
    ReactiveProperty<bool> isActive = new ReactiveProperty<bool>(false);
    System.IDisposable generateGarbageMicrotime;
    float garbageGenerateIntervalTime = 2;

    public Transform garbageDes;
    public GameObject[] garbageModels;
    // Start is called before the first frame update
    void Start()
    {
        OnDayStart();

        OnDayEnd();

        OnIsActiveChanged();
    }
    void GenerateGarbageRandomly()
    {
        generateGarbageMicrotime =
        Observable.Interval(System.TimeSpan.FromSeconds(garbageGenerateIntervalTime))
            .Subscribe(x =>
            {
                garbageGenerateIntervalTime = Random.Range(15, 25);
                int _modelNum = Random.Range(-1, garbageModels.Length);
                _modelNum = Mathf.Clamp(_modelNum, 0, garbageModels.Length - 1);
                var _garbage = Instantiate(garbageModels[_modelNum], transform.position, Quaternion.identity);
                var _dir = (garbageDes.position - transform.position).normalized;
                var _speed = Random.Range(1, 15.0f);
                _garbage.GetComponent<GarbagePresenter>().InitGarbage(_speed, _dir);
            });
    }

    void OnIsActiveChanged()
    {
        isActive
            .Where(x => x)
            .Subscribe(x =>
            {
                GenerateGarbageRandomly();
            });

        isActive
            .Where(x => !x)
            .Subscribe(x =>
            {
                if (generateGarbageMicrotime != null)
                {
                    generateGarbageMicrotime.Dispose();
                }
            });
           
    }

    void OnDayStart()
    {
        GameEvents.Sigton.onDayStart += () =>
        {
            isActive.Value = true;
        };
    }

    void OnDayEnd()
    {
        GameEvents.Sigton.onDayEnd += () =>
        {
            isActive.Value = false;
        };
    }

    private void OnDrawGizmos()
    {
        Handles.DrawDottedLine(transform.position, garbageDes.position,5);
    }
}
