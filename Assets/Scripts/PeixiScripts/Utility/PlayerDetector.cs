using System;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using UnityEngine.Events;

public class PlayerDetector : MonoBehaviour
{
    IObservable<Collider> onPlayerEnter;
    IObservable<Collider> onPlayerExit;

    public UnityEvent sendOnPlayerEnter;
    public UnityEvent sendOnPlayerExit;

    /// <summary>
    /// 玩家进入碰撞器
    /// </summary>
    public IObservable<Collider> OnPlayerEnter { get => onPlayerEnter; }
    /// <summary>
    /// 玩家离开碰撞器
    /// </summary>
    public IObservable<Collider> OnPlayerExit { get => onPlayerExit; }
    // Start is called before the first frame update
    void Start()
    {
        onPlayerEnter = ObservableTriggerExtensions.OnTriggerEnterAsObservable(gameObject);
        onPlayerEnter
            .Where(x => x.tag == "Player")
            .Subscribe(x =>
            {
                AssertExtension.NotNullRun(sendOnPlayerEnter, () =>
                {
                    sendOnPlayerEnter.Invoke();
                });

            });
        onPlayerExit = ObservableTriggerExtensions.OnTriggerExitAsObservable(gameObject);
        onPlayerExit
            .Where(x => x.tag == "Player")
            .Subscribe(x =>
            {
                AssertExtension.NotNullRun(sendOnPlayerExit, () =>
                {
                    sendOnPlayerExit.Invoke();
                });
            });
    }
}
