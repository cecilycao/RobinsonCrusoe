
public interface IPlayerInteractPresenter  
{
    void PlayerStartInteraction(PlayerInteractionType interact);
    /// <summary>
    /// 结束当前互动行为
    /// </summary>
    void PlayerEndInteraction();
}
