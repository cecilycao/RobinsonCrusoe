using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tags
{
    
}
public class SceneTags
{
    public const string gameLogScene = "GameLog";
    public const string testorScene = "TestorScene";
    public const string gameSystem = "GameSystem";
    public const string loadingScene = "LoadingScene";
}

public class InteractEventTags
{
    public const string onInteractBtnPressedWhenInteracting = "onInteractBtnPressedWhenInteracting";
    public const string onInteractBtnReleasedWhenInteracting = "onInteractBtnReleasedWhenInteracting";
}

public class PlotEventTags
{
    public const string npcFirstSicked = "npcFirstSicked";
    public const string playerFirstSicked = "playerFirstSicked";
}
public class PlayerEventTags
{
    public const string onFatigueReachMax = "onFatigueReachMax";
    public const string onHungerReachMax = "onHungerReachMax";
}

public class MechanismEventTags
{
    public const string onDayTimeOut = "onDayTimeOut";
}

/// <summary>
/// 更加方便获取interact config value
/// </summary>
public class InteractConfigKeys
{
    /// <summary>
    /// 加工食物饥饿值变化(默认值)
    /// </summary>
    public const string interact_processFood_hungerDecrea_default = "interact_processFood_hungerDecrea_default";
}