using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProcessPlantSample : MonoBehaviour,IFoodProcess
{
    string materialType = "FoodMaterial";
    string objectType = "FoodProcessPlant";
    int cost = 10;
    int hungerRestore = 5;
    public string FoodMaterialType => materialType;
    public int Cost => cost;
    public int HungerRestore => hungerRestore;
    public string InteractObjectType => objectType;

    public void EndInteractWithPlayer()
    {
        Mediator.Sigton.EndInteract();
    }

    public void OnEndProcessFood()
    {
        
    }

    public void OnStartProcessFood()
    {
        
    }

    public void StartInteractWithPlayer()
    {
        Mediator.Sigton.StartProcessFood(this);
    }
}
