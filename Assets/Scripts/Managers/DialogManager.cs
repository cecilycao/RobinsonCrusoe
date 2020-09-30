using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UniRx;

public class DialogManager : MonoBehaviour
{
    static DialogManager _instance;
    public static DialogManager Singelton
    {
        get => _instance;
        set
        {
            if (_instance==null)
            {
                _instance = value;
            }
        }
    }
    private void Awake()
    {
        _instance = this;
    }
    public void StartDialog(string npc)
    {
        print("start " + npc + "'s dialog tree");
        //延迟3秒结束对话 
        IDisposable _dia = null;
        _dia= Observable.Timer(TimeSpan.FromSeconds(3))
            .Subscribe(x =>
            {
                Mediator.Sigton.EndDialog();
                _dia.Dispose();
            });
    }
}
