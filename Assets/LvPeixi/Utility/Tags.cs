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
    public const string onFatigueValueIncreased = "onFatigueValueIncreased";
    public const string onHungerValueIncreased = "onHungerValueIncreased";
    public const string onPoisonChanged = "playerAttr_onPoisonChanged";
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
    public const string processFood_hungerDecrea_default = "interact_processFood_hungerDecrea_default";
    public const string posCollect_buildingMat_defaut = "interact_positiveCollect_buildingMaterialResourceCollectValue_default";
    public const string posCollect_foodMat_defaut = "interact_positiveCollect_foodMaterialResourceCollectValue_default";
    public const string negCollect_buildingMat_default = "interact_negativeCollect_buildingMaterialResourceCollectValue_default";
    public const string negCollect_foodMat_default = "interact_negativeCollect_foodMaterialResourceCollectValue_default";
    public const string restoreIsland_fatigueChange_default = "interact_restoreIsland_fatigueChange_defalut";
    public const string restoreIsland_hungerChange_default = "interact_restoreIsland_hungerChange_default";
}