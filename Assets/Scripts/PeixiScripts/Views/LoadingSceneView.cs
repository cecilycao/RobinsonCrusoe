using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class LoadingSceneView : MonoBehaviour
{
    public Slider progressSlider;
    public Text progressText;
     // Start is called before the first frame update
    void Start()
    {

        LoadManager.Singleton.LoadProgress
            //.Where(x => x < 0.9f)
            .Subscribe(x =>
            {
                progressSlider.value = x;
                progressText.text = (x * 100).ToString() + "%";
            });

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
