

public interface IInteractableNPC : IInteractable
{
    string NPCName { get; }
    void OnDialogStart();
    void OnDialogEnd();
}
