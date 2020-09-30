
public interface IInteractableResourceCollector:IInteractable
{
    int ResourceAccount { get; }
    string ResourceType { get; }
    void OnResourceCollectStart();
    void OnResourceCollectEnd();
}
