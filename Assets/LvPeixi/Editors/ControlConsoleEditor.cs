using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UniRx;

[CustomEditor(typeof(ControlConsole))]
public class ControlConsoleEditor : Editor
{
    ControlConsole control;
    bool showGuiTest;
    bool showPlayerTest;
    bool showGameEventTest;
    bool showAudioTest;
    private void OnEnable()
    {
        control = (ControlConsole)target;
    }
    public override void OnInspectorGUI()
    {
        showPlayerTest = EditorGUILayout.Foldout(showPlayerTest, "玩家功能测试");
        if (showPlayerTest)
        {
            if (GUILayout.Button("食材+20"))
        {
            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            inventory.FoodMaterial.Value += 20;
        }
            if (GUILayout.Button("建材+20"))
        {
            var inventory = FindObjectOfType<SimplePlayerInventoryPresenter>();
            inventory.BuildingMaterial.Value += 20;
        }
            if (GUILayout.Button("疲劳+20"))
        {
            var attr = FindObjectOfType<PlayerAttributePresenter>();
            attr.Fatigue.Value += 20;
        }
            if (GUILayout.Button("饥饿+20"))
        {
            var attr = FindObjectOfType<PlayerAttributePresenter>();
            attr.Hunger.Value += 20;
        }
            if (GUILayout.Button("毒性+2"))
            {
                GameEvents.Sigton.GetEvent<ReactiveProperty<int>>(PlayerEventTags.onPoisonChanged)
                    .Value += 2;
            }
        }

        #region//-----Gui events&funtions test-----
        showGuiTest = EditorGUILayout.Foldout(showGuiTest, "Gui功能测试");
        if (showGuiTest)
        {
            if (GUILayout.Button("测试Fish Game小游戏(只能点一次)"))
            {
                GUIEvents.Singleton.PlayerStartFishing.OnNext(true);
            }
        }
        #endregion

        showGameEventTest = EditorGUILayout.Foldout(showGameEventTest, "游戏事件测试");
        if (showGameEventTest)
        {
            if (GUILayout.Button("onDayStart.Invoke()"))
            {
                GameEvents.Sigton.onDayStart.Invoke();
            }

            if (GUILayout.Button("onDayEnd.Invoke()"))
            {
                GameEvents.Sigton.onDayEnd.Invoke();
            }

            if (GUILayout.Button("获得一天时长测试"))
            {
                var timeModel = FindObjectOfType<TimeSystemPresenter>().timeModel;
                Debug.Log(timeModel.dayLastTime);
            }

            if (GUILayout.Button("获取最大疲劳值"))
            {
                var maxFatigue = GameConfig.Singleton.PlayerConfig["hungerCeiling"];
                Debug.Log(maxFatigue);
            }

            if (GUILayout.Button("关上日记本"))
            {
                var diarymanager = FindObjectOfType<DiaryManager>();
                diarymanager.hideContentAfterClose();
            }
        }

        showAudioTest = EditorGUILayout.Foldout(showAudioTest, "声音测试");
        if (showAudioTest)
        {
            if (GUILayout.Button("声音播放测试"))
            {
                AudioManager.Singleton.PlayAudio("GameEvent_rainDay");
            }

            if (GUILayout.Button("声音停止测试"))
            {
                AudioManager.Singleton.PauseAudio("GameEvent_rainDay");
            }
        }
    }
}



