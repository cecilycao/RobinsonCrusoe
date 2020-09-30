using UniRx;

public interface IKeyboardInput 
{
    Subject<string> OnInteractBtnPressed { get;}
}
