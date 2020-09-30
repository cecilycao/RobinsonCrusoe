
public interface IInteractable 
{
    string InteractObjectType { get; }
    /// <summary>
    /// Player enter trigger时触发次方法
    /// </summary>
    void StartInteractWithPlayer();
    /// <summary>
    /// Player exit trigger时触发次方法
    /// </summary>
    void EndInteractWithPlayer();
}

