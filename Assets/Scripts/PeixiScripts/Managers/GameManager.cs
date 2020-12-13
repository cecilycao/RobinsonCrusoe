using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    private void Awake()
    {
        GameConfig.Singleton = new GameConfig();
        ///GUIEvents.Singleton = new GUIEvents();
  
    }

    private void Start()
    {
        GameEvents.Sigton.onGameEnd += () =>
        {
          
        };
    }
}
