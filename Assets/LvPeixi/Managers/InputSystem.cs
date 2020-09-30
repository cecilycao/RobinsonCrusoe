using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class InputSystem : MonoBehaviour, IKeyboardInput
{
    Subject<string> onInteractBtnPressed = new Subject<string>();
    public Subject<string> OnInteractBtnPressed { get => onInteractBtnPressed; }

    static IKeyboardInput _instance;
    public static IKeyboardInput Singleton
    {
        get => _instance;
        set
        {
            if (_instance == null)
            {
                _instance = value;
            }
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    private void Start()
    {
        Observable.EveryUpdate()
            .Where(x => Input.GetKeyDown(KeyCode.E))
            .Subscribe(x =>
            {
                onInteractBtnPressed.OnNext("e");
            });
    }
}
