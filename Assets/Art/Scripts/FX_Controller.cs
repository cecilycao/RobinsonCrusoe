using UnityEngine;
using UnityEditor;

public class FX_Controller : MonoBehaviour {
    public GameObject Storm;
    private ParticleSystem[] FX_Storm;

    public void StopParticle()
    {
        FX_Storm = Storm.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem storm in FX_Storm)
        {
            storm.Stop();
        }
    }

    public void PlayParticle()
    {
        FX_Storm = Storm.GetComponentsInChildren<ParticleSystem>();
        foreach(ParticleSystem storm in FX_Storm)
        {
            storm.Play();
        }
    }
}

[CustomEditor(typeof(FX_Controller))]               //很重要
public class FX_Controller_Editor : Editor {
    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        FX_Controller myScript = (FX_Controller)target;
        if(GUILayout.Button("Stop Particle")) {
            myScript.StopParticle();
        }
        if(GUILayout.Button("Play Particle")) {
            myScript.PlayParticle();
        }
    }
}

