using System.Collections;
using UniRx;

public interface IPlayerAttribute 
{
    ReactiveProperty<int> Fatigue { get; set; }
    ReactiveProperty<int> Hunger { get; set; }
}
