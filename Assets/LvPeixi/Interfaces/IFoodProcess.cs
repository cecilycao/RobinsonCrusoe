
public interface IFoodProcess : IInteractable
{
    string FoodMaterialType { get; }
    int Cost { get; }
    int HungerRestore { get; }
    bool HasFood { get; }
}
