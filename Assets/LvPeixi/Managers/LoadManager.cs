using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;

public class LoadManager:MonoBehaviour 
{
    private static LoadManager instance;
    public static LoadManager Singleton
    {
        get => instance;
        set
        {
            if (instance == null)
            {
                instance = value;
            }
        }
    }

    private static string theCurrentScene = "GameLog";
    private static string theNextScene = "Testor";
    private static float loadProgress = 0;
    private void Awake()
    {
        Singleton = this;
    }
    public string TheCurrentScene
    {
        get => theCurrentScene;
    }
    public string TheNextScene
    {
        get => theNextScene;
    }
    public float LoadProgress
    {
        get => loadProgress;
    }

    public void LoadTheNextScene(string sceneName)
    {
        SceneManager.UnloadSceneAsync(theCurrentScene);
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(sceneName);

        System.IDisposable asyncLoadMicrotine = null;
        asyncLoadMicrotine = Observable.EveryUpdate()
            .Subscribe(x =>
            {
                loadProgress = asyncOperation.progress;
            });
    }
}
