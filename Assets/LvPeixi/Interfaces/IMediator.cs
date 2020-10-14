
public interface IMediator 
{
    IPlayerInteractPresenter PlayerInteract { set; }

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
    void StartAddIsland(IslandBuilder builder);

    void StartRestoreIsland(IInteractableIsland island);
    void StartProcessFood(IFoodProcess foodProcess);
    void OpenDiary(Diary diary);

    void StartInteraction(IInteractableNPC npc);
    void StartInteraction(IInteractableResourceCollector collector);
    void StartInteraction(IIslandBuilder island);
    void StartInteraction(IFoodProcess foodProcess);
    void StartInteraction(IInteractableIsland island);
}
