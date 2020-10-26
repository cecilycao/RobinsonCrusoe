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
    Flowchart playerFlowchart;
    //public Flowchart playerFlowchart;
    /*
    public RectTransform SelfDialogPanel;
    public GameObject SelfDialogPlaceHolder;
    public RectTransform NPCDialogPanel;
    public GameObject NPCDialogPlaceHolder;
    */
    [Tooltip("固定对话（不受到好感度影响）的最后一天")]
    public int fixDialogEndDay;
    [Tooltip("所有Fungus接受的【对话】信息，不包括自言自语")]
    public List<string> ValidMessages;
    

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

        int newDay;
        playerFlowchart = null;
        foreach (SelfDialog dialog in SelfDialogs)
        {
            if (dialog.isPlayer)
            {
                playerFlowchart = dialog.flowchart;
            }
        }
        Assert.IsNotNull(playerFlowchart, "didn't find playerFlowchart");

        GameEvents.Sigton.timeSystem
        .Subscribe(_data =>
        {
            newDay = (int)_data.DayCount;
            time = (int)_data.TimeCountdown;
            //reset dialog count everyday
            if(newDay != day)
            {
                dialogCount = 0;
                day = newDay;
            }
            
            foreach (SelfDialog dialog in SelfDialogs)
            {
                dialog.sendMessageToFlowchart(day, time);
            }
        });

        GameEvents.Sigton.GetEvent<Subject<SubjectArg>>(InteractEventTags.showMonologue)
            .Subscribe(x =>
            {
                print(x.senderSignature);
                playerFlowchart.SetStringVariable("prompt", x.senderSignature);
                playerFlowchart.SendFungusMessage("ShowPrompt");
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
        Debug.Log("Start dialog " + dialogCount + "with " + npc);
        foreach(CharacterFlowchart item in Dialogflowcharts)
        {
            if(item.getName() == npc)
            {
                Debug.Log("Send message to " + npc);
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
        if (day <= fixDialogEndDay || day > SickManager.Instance.PlayerSickedDay + 1)
        {
            //按照日期分配对话 0-7 and 15 - 
            message = day + "-" + dialogCount;
            
        } else if(day <= SickManager.Instance.PlayerSickedDay)
        {
            //按照好感度分配对话
            message = item.NPCcomponent.preference + "/" + dialogCount;
        } else if(day == SickManager.Instance.PlayerSickedDay + 1)
        {
            //按照是否救助分配对话
            if (SickManager.Instance.isPlayerSaved)
            {
                message = (SickManager.Instance.PlayerSickedDay + 1) + "notSavedDefault";
            } else
            {
                if (dialogCount == 1)
                {
                    message = (SickManager.Instance.PlayerSickedDay + 1) + "Saved";
                }
                else
                {
                    message = (SickManager.Instance.PlayerSickedDay + 1) + "SavedDefault";
                }
            }
        }
        if (ValidMessages.Contains(message) && dialogCount == 1)
        {
            //a valid dialog, not default
            item.NPCcomponent.addPreference();
            var attr = FindObjectOfType<PlayerAttributePresenter>();
            attr.Fatigue.Value -= 10;
        }
        message = processMessage(message);
        Debug.Log("send to flowchart " + message);
        item.flowchart.SendFungusMessage(message);
    }

    public string processMessage(string message)
    {
        if (ValidMessages.Contains(message))
        {
            return message;
        } else
        {
            string newMsg = "";
            if (day <= fixDialogEndDay)
            {
                newMsg = message.Split('-')[0]+"-default";
            }
            else
            {
                newMsg = message.Split('/')[0] + "-default";
            }
            if (!ValidMessages.Contains(newMsg))
            {
                newMsg = "default";
            }
            //send
            return newMsg;
        }
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

