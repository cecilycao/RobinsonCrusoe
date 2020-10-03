using UnityEngine;
using System;
using UniRx;

/// <summary>
/// GameEvents只负责两个系统之间传递数据，没有任何协调、判定
/// </summary>
public class GameEvents : MonoBehaviour
{
    static GameEvents events;
    public Action onGameStart;
    public Action onGameEnd;
    public Action onDayStart;
    public Action onDayEnd;

    public Subject<ITimeSystemData> timeSystem = new Subject<ITimeSystemData>();
    public Action<int, int> onIslandMerged;

    //----------浮岛事件---------------
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

    //----------Interactions-------------
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

    public Action<IInteractable> onInteractStart;
    public Action onInteractEnd;

    //----------Weather System--------------
    public Action OnRainStart;
    public Action OnRainEnd;
    public Action OnStormStart;
    public Action OnStormEnd;



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
}
