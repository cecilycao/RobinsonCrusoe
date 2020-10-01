
public interface IFoodProcess : IInteractable
{
    string FoodMaterialType { get; }
    int Cost { get; }
    int HungerRestore { get; }

    void OnStartProcessFood();
    void OnEndProcessFood();
}
