using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using Fungus;

public class DialogManager : MonoBehaviour
{
    Flowchart flowchart;
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
        flowchart = GetComponentInChildren<Flowchart>();
        Assert.IsNotNull(flowchart);
    }
    public void StartDialog(string npc)
    {
        flowchart.SendFungusMessage(npc);
    }
    public void EndDialog()
    {
        Mediator.Sigton.EndDialog();
    }
}
