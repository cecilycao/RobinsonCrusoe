
public interface IInteractableResourceCollector:IInteractable
{
    int ResourceAccount_Food { get; }
    int ResourceAccount_Build { get; }
    string ResourceType { get; }
}
