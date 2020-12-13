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
    /// <summary>
    /// 与道具互动时，玩家按下Interact key时触发此Subject
    /// </summary>
    public const string onInteractBtnPressed = "interact_onInteractBtnPressed";
    /// <summary>
    /// 与道具互动时，玩家松开Interact key时触发此Subject
    /// </summary>
    public const string onInteractBtnReleased = "interact_onInteractBtnReleased";
    /// <summary>
    /// 玩家和道具接触时触发此Subject
    /// </summary>
    public const string onPlayerContactStarted = "interact_onPlayerContactStarted";
    /// <summary>
    /// 玩家和道具脱离接触时触发此Subject
    /// </summary>
    public const string onPlayerContactEnded = "interact_onPlayerContactEnded";
    /// <summary>
    /// 玩家成功完成互动行为时触发此Subject
    /// </summary>
    public const string onInteractionCompleted = "interact_onInteractionCompleted";
    /// <summary>
    /// 发送玩家在心中的独白
    /// </summary>
    public const string showMonologue = "interact_showMonologue";
}

public class PlotEventTags
{
    public const string npcFirstSicked = "npcFirstSicked";
    public const string playerFirstSicked = "playerFirstSicked";
}
public class PlayerEventTags
{
    public const string onFatigueReachMax = "playerAttr_onFatigueReachMax";
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

public class AudioConfigKeys
{
    public const string GameEvent_rainDay = "GameEvent_rainDay";
    public const string GameEvent_stormComing = "GameEvent_stormComing";
    public const string GameEvent_sunnyDay = "GameEvent_sunnyDay";
    public const string Interact_build_restoreIsland_processFoodComplete = "Interact_build_restoreIsland_processFoodComplete";
    public const string Interact_islandBuilding = "Interact_islandBuilding";
    public const string Interact_islandRestoring = "Interact_islandRestoring";
    public const string Interact_positiveCollectResourceComplete = "Interact_positiveCollectResourceComplete";
    public const string Interact_processingFood = "Interact_processingFood";
    public const string Interact_resourceCollectComplete = "Interact_resourceCollectComplete";
    public const string Interact_startContactTipSound = "Interact_startContactTipSound";
    public const string Player_fatigueValueIncreased = "Player_fatigueValueIncreased";
    public const string Player_hungerValueIncreased = "Player_hungerValueIncreased";
    public const string PlayerBehavior_footstep = "PlayerBehavior_footstep";
    public const string UI_diaryBtnPauseGame = "UI_diaryBtnPauseGame";
    public const string UI_diaryBtnRusumeGame = "UI_diaryBtnRusumeGame";
    public const string UI_openDiary = "UI_openDiary";
    public const string UI_turnDiaryPage = "UI_turnDiaryPage";
}