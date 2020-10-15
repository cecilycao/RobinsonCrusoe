using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IGameManager
{
    private void Awake()
    {
        GameConfig.Singleton = new GameConfig();
    }
    private void Start()
    {
       
    }
}
