using UniRx;
public interface IGUIEvents 
{
    ReactiveProperty<int> FoodMaterial { get; set; }
    ReactiveProperty<int> Fatigue { get; set; }
    Subject<string> BroadcastInteractTipMessage { get;}
    ReactiveProperty<int> BuildingMaterial { get; set; }
    ReactiveProperty<int> Hunger { get; set; }
}
