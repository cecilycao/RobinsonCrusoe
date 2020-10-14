using UniRx;
public interface IGUIEvents 
{
    ReactiveProperty<int> FoodMaterial { get; set; }
    ReactiveProperty<int> Fatigue { get; set; }
    Subject<string> BroadcastInteractTipMessage { get;}
    /// <summary>
    /// 玩家在互动等待时，在头顶显示此UI
    /// 传递progress的比率
    /// </summary>
    Subject<float> InteractionProgressBar { get; }
    ReactiveProperty<int> BuildingMaterial { get; set; }
    ReactiveProperty<int> Hunger { get; set; }
    
    Subject<bool> PlayerStartFishing { get; }
    /// <summary>
    /// deliver player's fishing result;
    /// </summary>
    Subject<bool> PlayerEndFishing { get; }
}
