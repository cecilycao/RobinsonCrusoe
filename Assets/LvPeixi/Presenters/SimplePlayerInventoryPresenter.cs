using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SimplePlayerInventoryPresenter : MonoBehaviour
{
    [SerializeField]
    private SimplePlayerInventoryModel model = new SimplePlayerInventoryModel();


    [SerializeField]
    int foodMaterial_test;
    [SerializeField]
    int buildMaterial_test;

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

       
    }
    private void Start()
    {
        var config = GameConfig.Singleton.PlayerConfig;
        GUIEvents.Singleton.FoodMaterial = model.foodMaterial;
        GUIEvents.Singleton.BuildingMaterial = model.buildingMaterial;
        model.buildingMaterialCeiling = (int)config["buildingMaterialCeiling"];
        model.foodMaterialCeiling = (int)config["foodMaterialCeiling"];

        model.foodMaterial
            .Subscribe(x =>
            {
                foodMaterial_test = x;
            });
        model.buildingMaterial
            .Subscribe(x =>
            {
                buildMaterial_test = x;
            });
                


    }
}
