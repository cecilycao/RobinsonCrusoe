using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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
        }

        showAudioTest = EditorGUILayout.Foldout(showAudioTest, "声音测试");
        if (showAudioTest)
        {
            if (GUILayout.Button("声音播放测试"))
            {
                AudioManager.Singleton.PlayAudio("Interact_islandRestoring");
            }

            if (GUILayout.Button("声音停止测试"))
            {
                AudioManager.Singleton.PauseAudio("Interact_islandRestoring");
            }
        }
    }
}



