using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// GameEvents只负责两个系统之间传递数据，没有任何协调、判定
/// </summary>
public class GameEvents : MonoBehaviour
{
    public enum EventDictionaryType
    {
        InteractEvent,
        PlotEvent,
        MechanismEvent
    }

    static GameEvents events;

    #region//Private variables
    private Dictionary<string, Subject<SubjectArg>> interactEventDic = new Dictionary<string, Subject<SubjectArg>>();
    private Dictionary<string, Subject<SubjectArg>> plotEventDic = new Dictionary<string, Subject<SubjectArg>>();
    private Dictionary<string, Subject<SubjectArg>> mechanismEventDic = new Dictionary<string, Subject<SubjectArg>>();
    private Dictionary<string, Subject<SubjectArg>> playerEventDic = new Dictionary<string, Subject<SubjectArg>>();
    private Dictionary<string, Subject<SubjectArg>> propEventDic = new Dictionary<string, Subject<SubjectArg>>();
    private Hashtable playerEventTab = new Hashtable();


    private Dictionary<EventDictionaryType, Dictionary<string, Subject<SubjectArg>>> eventDicManager = 
        new Dictionary<EventDictionaryType, Dictionary<string, Subject<SubjectArg>>>();
    #endregion

    #region-----Public properties && methods
    /// <summary>
    /// 互动事件
    /// </summary>
    public Dictionary<string, Subject<SubjectArg>> InteractEventDictionary
    {
        get => interactEventDic;
    }
    /// <summary>
    /// 剧情事件
    /// </summary>
    public Dictionary<string,Subject<SubjectArg>> PlotEventDictionary
    {
        get => plotEventDic;
    }
    /// <summary>
    /// 游戏机制事件
    /// </summary>
    public Dictionary<string,Subject<SubjectArg>> MechanismEventDictionary
    {
        get => mechanismEventDic;
    }
    /// <summary>
    /// 玩家事件
    /// </summary>
    public Dictionary<string, Subject<SubjectArg>> PlayerEventDictionary
    {
        get => playerEventDic;
    }
    /// <summary>
    /// 道具事件
    /// </summary>
    public Dictionary<string, Subject<SubjectArg>> PropertyEventDictionay
    {
        get => propEventDic;
    }
    public void RegisterEvent(EventDictionaryType type,string eventKey)
    {
        var _theTargetDic = eventDicManager[type];
        _theTargetDic.Add(eventKey,new Subject<SubjectArg>());
    }

    public object GetEvent(EventDictionaryType type,string eventKey)
    {
        return 1;
    }

    public object GetEvent(string eventKey)
    {
        return 1;
    }
    #endregion

    #region-----重要游戏事件-----
    public Action onGameStart;
    public Action onGameEnd;
    public Action onDayStart;
    public Action onDayEnd;
    public Subject<ITimeSystemData> timeSystem = new Subject<ITimeSystemData>();

    public Subject<int> onFatigueReachMax = new Subject<int>();
    public Subject<int> onHungerReachZero = new Subject<int>();

    public Subject<int> onNPCSicked = new Subject<int>();
    public Subject<int> onPlayerSicked = new Subject<int>();
    #endregion

    #region//----------浮岛事件-----------
    /// <summary>
    /// 浮岛合并时
    /// </summary>
    public Action<int, int> onIslandMerged;
    /// <summary>
    /// 玩家修复浮岛时触发
    /// </summary>
    public Action onIslandRestored;
    /// <summary>
    /// 玩家新建浮岛时触发
    /// </summary>
    public Action onIslandCreated;
    /// <summary>
    /// 玩家站在需要修复的岛上时触发
    /// </summary>
    public Action<bool> onTheBrokenIsland;
    /// <summary>
    /// 玩家站在毁坏的岛上时触发
    /// </summary>
    public Action onTheDestroyedIsland;

    public Action onNPCIslandAppear;
    public Action onNPCIslandCombined;

    /// 玩家站在坏掉并且沉没的岛上时触发
    /// </summary>
    public Action<Hashtable> onIslandSinkWhenPlayerOnIt;
    #endregion

    #region//----------Interactions-------------

    /// <summary>
    /// 对话展开时触发,需要一个string作为dialogID
    /// </summary>
    public Action<string> onDialogStart;
    public Action onDialogEnd;
    /// <summary>
    /// 使用食品工厂加工完成时触发
    /// </summary>
    public Action onFoodCreated;
    /// <summary>
    /// 捡起资源时触发
    /// </summary>
    public Action<string, int> onResourceCollected;
    /// <summary>
    /// 互动开始
    /// </summary>
    public Action<IInteractable> onInteractStart;
    /// <summary>
    /// 互动结束
    /// </summary>
    public Action onInteractEnd;
    /// <summary>
    /// 玩家休息
    /// </summary>
    public Action onPlayerRest;
    #endregion

    public Action OnDiaryStart;
    public Action OnDiaryEnd;

    #region//----------Weather System--------------

    public Action OnRainStart;
    public Action OnRainEnd;
    public Action OnStormStart;
    public Action OnStormEnd;

    #endregion

    #region----------For init----------

    public static GameEvents Sigton
    {
        get => events;
        set
        {
            if (events == null)
            {
                events = value;
            }
        }
    }

    private void Awake()
    {
        events = this;

        InitEventDicManager();

        InitNecessaryEvents();  
    }
    #endregion
    void InitEventDicManager()
    {
        eventDicManager.Add(EventDictionaryType.InteractEvent, interactEventDic);
        eventDicManager.Add(EventDictionaryType.MechanismEvent, mechanismEventDic);
        eventDicManager.Add(EventDictionaryType.PlotEvent, plotEventDic);
    }

    void InitNecessaryEvents()
    {
        RegisterEvent(EventDictionaryType.InteractEvent, InteractEventTags.onInteractBtnPressedWhenInteracting);
        RegisterEvent(EventDictionaryType.InteractEvent, InteractEventTags.onInteractBtnReleasedWhenInteracting);

        RegisterEvent(EventDictionaryType.PlotEvent,PlotEventTags.playerFirstSicked);
        RegisterEvent(EventDictionaryType.PlotEvent, PlotEventTags.npcFirstSicked);
        RegisterEvent(EventDictionaryType.PlotEvent, PlayerEventTags.onFatigueReachMax);

        RegisterEvent(EventDictionaryType.MechanismEvent, MechanismEventTags.onDayTimeOut);
        
    }

}
public struct SubjectArg
{
    /// <summary>
    /// subject的名字
    /// </summary>
    string subjectName;
    /// <summary>
    /// 携带信息
    /// </summary>
    object subjectMes;
    public SubjectArg(string m_subjectName):this()
    {
        subjectName = m_subjectName;
    }
    public SubjectArg(string m_subjectName,object m_subjectMes)
    {
        subjectName = m_subjectName;
        subjectMes = m_subjectMes;
    }
}

