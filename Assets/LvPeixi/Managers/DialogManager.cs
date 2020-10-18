using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using Fungus;

public class DialogManager : MonoBehaviour
{
    static DialogManager _instance;

    int day = 0;
    int dialogCount = 0;

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

    private void Start()
    {
        GameEvents.Sigton.timeSystem
        .Subscribe(_data =>
        {
            day = (int)_data.DayCount;
            
        });
    }

    private void Awake()
    {
        _instance = this;
        
    }
    public void StartDialog(string npc)
    {
        dialogCount++;
        //todo: get flowchart by npc name
        Flowchart flowchart = GetComponentInChildren<Flowchart>();
        Assert.IsNotNull(flowchart);
        sendMessage(flowchart);
    }
    public void EndDialog()
    {
        Mediator.Sigton.EndDialog();
    }

    public void sendMessage(Flowchart flowchart)
    {
        string message = day + "-" + dialogCount;
        Debug.Log("send to flowchart " + message);
        flowchart.SendFungusMessage(message);
    }
}
