using UniRx;
using UnityEngine;
[System.Serializable]
public class SimplePlayerInventoryModel
{
    public ReactiveProperty<int> foodMaterial = new ReactiveProperty<int>();
    public ReactiveProperty<int> buildingMaterial = new ReactiveProperty<int>();
    [Header("-----食材容量上限-----")]
    public int foodMaterialCeiling;
    [Header("-----建材容量上限-----")]
    public int buildingMaterialCeiling;
}
