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
        GUIEvents.Singleton.Fatigue
            .Subscribe(x =>
            {
                fatigue.text = x.ToString();
            });
        GUIEvents.Singleton.FoodMaterial
            .Subscribe(x =>
            {
                foodMaterial.text = x.ToString();
            });

        GUIEvents.Singleton.BuildingMaterial
            .Subscribe(x =>
            {
                buildingMaterial.text = x.ToString();
            });

        GUIEvents.Singleton.Hunger
            .Subscribe(x =>
            {
                hunger.text = x.ToString();
            });
    }
}
