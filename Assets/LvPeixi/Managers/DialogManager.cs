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

    public CharacterFlowchart[] Dialogflowcharts;
    public SelfDialog[] SelfDialogs;
    //public Flowchart playerFlowchart;
    /*
    public RectTransform SelfDialogPanel;
    public GameObject SelfDialogPlaceHolder;
    public RectTransform NPCDialogPanel;
    public GameObject NPCDialogPlaceHolder;
    */
    [Tooltip("固定对话（不受到好感度影响）的最后一天")]
    public int fixDialogEndDay;

    [SerializeField]
    int day = 0;
    [SerializeField]
    int time = 0;
    [SerializeField]
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
        foreach (SelfDialog dialog in SelfDialogs)
        {
            Assert.IsNotNull(dialog.flowchart);
        }

        GameEvents.Sigton.timeSystem
        .Subscribe(_data =>
        {
            day = (int)_data.DayCount;
            time = (int)_data.TimeCountdown;
            foreach (SelfDialog dialog in SelfDialogs)
            {
                dialog.sendMessageToFlowchart(day, time);
            }
            //sendMessage(playerFlowchart, day.ToString());
            //reset dialog count everyday
            dialogCount = 0;
        });
    }

    /*
    private void Update()
    {
        
        Vector3 targetPosPlayer = Camera.main.WorldToScreenPoint(SelfDialogPlaceHolder.transform.position);
        SelfDialogPanel.position = targetPosPlayer;
        Vector3 targetPosNPC = Camera.main.WorldToScreenPoint(NPCDialogPlaceHolder.transform.position);
        NPCDialogPanel.position = targetPosNPC;
    }*/
    
    public void StartDialog(string npc)
    {
        dialogCount++;
        
        foreach(CharacterFlowchart item in Dialogflowcharts)
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

