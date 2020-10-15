using System.Collections;
using UnityEngine;
using System;
using UniRx;

/// <summary>
/// GameEvents只负责两个系统之间传递数据，没有任何协调、判定
/// </summary>
public class GameEvents : MonoBehaviour
{
    static GameEvents events;

    #region-----重要游戏事件-----
    public Action onGameStart;
    public Action onGameEnd;
    public Action onDayStart;
    public Action onDayEnd;
    public Subject<ITimeSystemData> timeSystem = new Subject<ITimeSystemData>();
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
    }
    #endregion
}
