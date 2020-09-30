using System.Collections;
using System;
using UniRx;

[Serializable]
public class NPCModel 
{
    public string npcName;
    /// <summary>
    /// 好感度
    /// </summary>
    public ReactiveProperty<int> favorability = new ReactiveProperty<int>();
    public string interactObjectType = "NPC";
}
