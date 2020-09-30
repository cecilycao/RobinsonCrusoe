
public interface IMediator 
{
    /// <summary>
    /// 请求开始对话互动
    /// </summary>
    /// <param name="npc"></param>
    void StartDialog(IInteractableNPC npc);
    void EndDialog();
    /// <summary>
    /// 请求开始收集互动
    /// </summary>
    /// <param name="collector"></param>
    void StartResourceCollect(IInteractableResourceCollector collector);
    void EndInteract();
    void StartAddIsland();
    void StartRestoreIsland(IInteractableIsland island);
    void StartProcessFood();
}
