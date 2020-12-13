using UniRx;

public interface IKeyboardInput 
{
    Subject<string> OnInteractBtnPressed { get;}
    Subject<string> OnInteractBtnReleased { get; }
    Subject<string> OnInteractBtnPressing { get; }
}
