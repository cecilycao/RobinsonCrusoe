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