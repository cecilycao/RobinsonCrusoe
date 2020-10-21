using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Assertions;
using UniRx;
using Fungus;

/**
 * 发给Fungus的消息格式：
 * 固定对话时期：天数-今日对话次数
 * 好感度对话时期：好感度-今日对话次数
**/
public class DialogManager : MonoBehaviour
{
    static DialogManager _instance;

    public CharacterFlowchart[] flowcharts;
    public Flowchart playerFlowchart;
    public RectTransform SelfDialogPanel;
    public GameObject SelfDialogPlaceHolder;

    [Tooltip("固定对话（不受到好感度影响）的最后一天")]
    public int fixDialogEndDay;
    
    int day = 0;
    int time = 0;
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

    private void Awake()
    {
        _instance = this;

    }

    private void Start()
    {
        Assert.IsNotNull(playerFlowchart);

        GameEvents.Sigton.timeSystem
        .Subscribe(_data =>
        {
            day = (int)_data.DayCount;
            time = (int)_data.TimeCountdown;
            sendMessage(playerFlowchart, day.ToString());
            //reset dialog count everyday
            dialogCount = 0;
        });
    }

    private void Update()
    {
        
        Vector3 targetPos = Camera.main.WorldToScreenPoint(SelfDialogPlaceHolder.transform.position);
        SelfDialogPanel.position = targetPos;
    }
    
    public void StartDialog(string npc)
    {
        dialogCount++;
        
        foreach(CharacterFlowchart item in flowcharts)
        {
            if(item.getName() == npc)
            {
                sendMessageToNPC(item);
            }
        }
        
    }
    public void EndDialog()
    {
        Mediator.Sigton.EndDialog();
    }

    public void sendMessageToNPC(CharacterFlowchart item)
    {
        Assert.IsNotNull(item.flowchart);
        string message = "";
        if (day <= fixDialogEndDay)
        {
            message = day + "-" + dialogCount;
            
        } else
        {
            message = item.NPCcomponent.preference + "/" + dialogCount;
        }
        Debug.Log("send to flowchart " + message);
        item.flowchart.SendFungusMessage(message);
    }

    public void sendMessage(Flowchart flowchart, string message)
    {
        flowchart.SendFungusMessage(message);
    }
}

[Serializable]
public class CharacterFlowchart
{
    public NPCSample NPCcomponent;
    public Flowchart flowchart;

    public string getName()
    {
        return NPCcomponent.NPCName;
    }
}
