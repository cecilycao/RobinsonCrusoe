using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameEventAudioViewer : MonoBehaviour
{
    int previousHunger = 0;
    int previousFatigue = 0;
    private void Start()
    {
        WhenHungerValueIncreased();     
    }
    void WhenHungerValueIncreased()
    {
        GUIEvents.Singleton.Hunger
        .Where(x => x > previousHunger)
        .Subscribe(x =>
        {
            AudioManager.Singleton.PlayAudio("Player_hungerValueIncreased");
            previousHunger = x;
        });
    }
    void WhenFatigueValueIncreased()
    {
        GUIEvents.Singleton.Fatigue
            .Where(x => x > previousFatigue)
            .Subscribe(x =>
            {
                AudioManager.Singleton.PlayAudio("Player_FatigueValueIncreased");
            });
    }
}
