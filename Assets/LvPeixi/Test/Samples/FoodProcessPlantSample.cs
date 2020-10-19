using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodProcessPlantSample : MonoBehaviour,IFoodProcess
{
    private string materialType = "FoodMaterial";
    private string objectType = "FoodProcessPlant";
    private int cost = 10;
    private int hungerRestore = 5;
    [SerializeField]
    private bool hasFood = false;
    public GameObject Icon;
    public string FoodMaterialType => materialType;
    public int Cost => cost;
    public int HungerRestore => hungerRestore;
    public string InteractObjectType => objectType;
    public bool HasFood => hasFood;

    private void Update()
    {
        
    }

    public void EndContact()
    {
        Mediator.Sigton.EndInteract();
    }

    public void EndInteract(object result)
    {
        hasFood = (bool)result;
    }

    public void OnEndProcessFood()
    {
        
    }

    public void OnStartProcessFood()
    {
        
    }

    public void StartContact()
    {
        Mediator.Sigton.StartInteraction(this);
    }

    public void StartInteract()
    {
        
    }

    public void ShowIcon()
    {
        Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position);
        Icon.SetActive(true);
    }


    public void HideIcon()
    {
        Icon.SetActive(false);
    }

}
