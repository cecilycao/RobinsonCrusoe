using UniRx;
public class SimplePlayerInventoryModel
{
    public ReactiveProperty<int> foodMaterial = new ReactiveProperty<int>();
    public ReactiveProperty<int> buildingMaterial = new ReactiveProperty<int>();
}
