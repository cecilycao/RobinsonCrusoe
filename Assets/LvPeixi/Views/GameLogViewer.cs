using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameLogViewer : MonoBehaviour
{
    [SerializeField]
    private Button startGameBtn;
    [SerializeField]
    private Button settingGameBtn;
    // Start is called before the first frame update
    void Start()
    {
        startGameBtn.onClick.AsObservable()
            .Subscribe(x =>
            {
                LoadManager.Singleton.LoadTheNextScene("Testor");
            });

        settingGameBtn.onClick.AsObservable()
            .Subscribe(x =>
            {
                print("Setting panel is not completed");
            });
    }
}
