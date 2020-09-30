
public interface IInteractableIsland : IInteractable
{
    string MaterialType { get; }
    int MaterialCost { get; }
    void OnIslandRestoreStart();
    void OnIslandRestoreEnd();

}
