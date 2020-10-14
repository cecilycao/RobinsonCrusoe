using UniRx;

public interface ITimeSystemData 
{
    float DayCount { get; }
    float TimeCountdown { get;}
    bool IsDay { get; }
}
