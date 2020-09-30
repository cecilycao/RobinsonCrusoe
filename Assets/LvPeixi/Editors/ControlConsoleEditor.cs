using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ControlConsole))]
public class ControlConsoleEditor : Editor
{
    ControlConsole control;
    private void OnEnable()
    {
        control = (ControlConsole)target;
    }
    public override void OnInspectorGUI()
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
}



