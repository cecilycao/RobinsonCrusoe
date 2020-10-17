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
    [SerializeField]
    private Slider fatigueSlider;
    [SerializeField]
    private Slider hungerSlider;
    [SerializeField]
    private Slider foodMaterialSlider;
    [SerializeField]
    private Slider buildingMaterialSlider;
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
                float _value = x / 100.0f;
                fatigueSlider.value = _value;
            });

        //监视玩家饥饿值
        GUIEvents.Singleton.Hunger
            .Subscribe(x =>
            {
                hungerSlider.value = x / 100.0f;
            });

        //监视玩家食材
        GUIEvents.Singleton.FoodMaterial
            .Subscribe(x =>
            {
                float maxFoodMaterial = GameConfig.Singleton.PlayerConfig["foodMaterialCeiling"];
                float _value = x / maxFoodMaterial;
                foodMaterialSlider.value = _value;
            });

        //监视玩家建材
        GUIEvents.Singleton.BuildingMaterial
            .Subscribe(x =>
            {
                float maxBuildingMaterial = GameConfig.Singleton.PlayerConfig["buildingMaterialCeiling"];
                float _value = x / maxBuildingMaterial;
                buildingMaterialSlider.value = _value;
            });
          
    }
}
    