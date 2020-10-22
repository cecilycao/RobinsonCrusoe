using Fungus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDialog : MonoBehaviour
{
    public bool isPlayer;
    public NPCSample NPCcomponent;
    public Flowchart flowchart;
    public RectTransform DialogPanel;
    public GameObject DialogPlaceHolder;

    private void Update()
    {
        Vector3 targetPos = Camera.main.WorldToScreenPoint(DialogPlaceHolder.transform.position);
        DialogPanel.position = targetPos;
    }

    public string getName()
    {
        return NPCcomponent.NPCName;
    }

    public void sendMessageToFlowchart(int day, int time)
    {
        string message = day + "";
        flowchart.SendFungusMessage(message);
    }


    //messageID 
    //1: 先去做别的事吧。
    //2: 他看起来没什么大问题，先去做别的事吧。
    //3: 看来新增岛块不能怠慢……
    public void stopTalk(int messageID)
    {
        flowchart.SendFungusMessage("StopTalk" + messageID);
    }
}

