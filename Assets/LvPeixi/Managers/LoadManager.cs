using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UniRx;
using System;

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
    private ReactiveProperty<float> loadProgress = new ReactiveProperty<float>(0);

    public float progress;
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
    public ReactiveProperty<float> LoadProgress
    {
        get => loadProgress;
    }

    public void LoadTheNextScene(string sceneName)
    {
        SceneManager.LoadSceneAsync("LoadingScene", LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync(theCurrentScene);
        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Additive);

        asyncOperation.allowSceneActivation = false;
        
        System.IDisposable asyncLoadMicrotine = null;
        asyncLoadMicrotine = Observable.EveryUpdate()
            .Subscribe(x =>
            {
                loadProgress.Value = asyncOperation.progress;
                if (asyncOperation.progress >= .9f)
                {
                    Observable.Timer(TimeSpan.FromSeconds(2))
                     .First()
                     .Subscribe(y =>
                     {
                         SceneManager.UnloadSceneAsync("LoadingScene");
                         asyncOperation.allowSceneActivation = true;
                     }).AddTo(this);
                    asyncLoadMicrotine.Dispose();
                }
            }).AddTo(this);
    }
}
