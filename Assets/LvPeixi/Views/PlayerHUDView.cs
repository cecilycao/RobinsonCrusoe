using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UniRx;

public class PlayerHUDView : MonoBehaviour
{
    public Text interactTipMsg;
    public Text foodMaterial;
    public Text buildingMaterial;
    public Text fatigue;
    public Text hunger;
    // Start is called before the first frame update
    void Start()
    {
        GUIEvents.Singleton.BroadcastInteractTipMessage
            .Subscribe(x =>
            {
                interactTipMsg.text = x;
            });

        //监视玩家疲劳值
        GUIEvents.Singleton.Fatigue
            .Subscribe(x =>
            {
                fatigue.text = x.ToString();
            });

        //监视玩家食材
        GUIEvents.Singleton.FoodMaterial
            .Subscribe(x =>
            {
                foodMaterial.text = x.ToString();
            });

        //监视玩家建材
        GUIEvents.Singleton.BuildingMaterial
            .Subscribe(x =>
            {
                buildingMaterial.text = x.ToString();
            });
          
        //监视玩家饥饿值
        GUIEvents.Singleton.Hunger
            .Subscribe(x =>
            {
                hunger.text = x.ToString();
            });
    }
}
    