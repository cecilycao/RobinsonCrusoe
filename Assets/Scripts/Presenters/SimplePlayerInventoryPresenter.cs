using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class SimplePlayerInventoryPresenter : MonoBehaviour
{
    SimplePlayerInventoryModel model = new SimplePlayerInventoryModel();
    public ReactiveProperty<int> FoodMaterial
    {
        get => model.foodMaterial;
    }
    public ReactiveProperty<int> BuildingMaterial
    {
        get => model.foodMaterial;
    }
    private void Start()
    {
        GUIEvents.Singleton.FoodMaterial = model.foodMaterial;
    }
}
