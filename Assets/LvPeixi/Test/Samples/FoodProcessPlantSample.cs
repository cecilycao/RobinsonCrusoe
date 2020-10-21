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

    public Vector3 IconOffset = new Vector3(0, 7, 0);

    private void Start()
    {
        //Icon = FindObjectOfType<IconManager>().ProcessFoodIcon;
        //if(Icon == null)
        //{
        //    Debug.LogError("Icon haven't been assigned to IconManager");
        //}
    }

    private void Update()
    {
        //Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
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
        Icon.transform.position = Camera.main.WorldToScreenPoint(transform.position + IconOffset);
        Mediator.Sigton.StartInteraction(this);
    }

    public void StartInteract()
    {
        
    }

    public void ShowIcon()
    {
        Icon.SetActive(true);
    }


    public void HideIcon()
    {
        Icon.SetActive(false);
    }

}
