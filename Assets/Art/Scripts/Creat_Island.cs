using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Creat_Island : MonoBehaviour
{
    public GameObject Island;
    public Transform parent;
    public void Creat()
    {
        Instantiate(Island, parent);
    }
}


[CustomEditor(typeof(Creat_Island))]
public class Creat_Island_Editor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        Creat_Island myScript = (Creat_Island)target;
        if(GUILayout.Button("CreatIsland")) {
            myScript.Creat();
        }
    }
}