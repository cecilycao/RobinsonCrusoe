using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SimplePlayerInventoryPresenter : MonoBehaviour
{
    [SerializeField]
    private SimplePlayerInventoryModel model = new SimplePlayerInventoryModel();
    public ReactiveProperty<int> FoodMaterial 
    {
        get => model.foodMaterial;
    }
    public ReactiveProperty<int> BuildingMaterial
    {
        get => model.buildingMaterial;
    }
    private void Awake()
    {
        model.foodMaterial.Value = 0;
        model.buildingMaterial.Value = 0;

        GUIEvents.Singleton.FoodMaterial = model.foodMaterial;
        GUIEvents.Singleton.BuildingMaterial = model.buildingMaterial;
    }
    private void Start()
    {
        var config = GameConfig.Singleton.PlayerConfig;
        model.buildingMaterialCeiling = (int)config["buildingMaterialCeiling"];
        model.foodMaterialCeiling = (int)config["foodMaterialCeiling"];
    }
}
