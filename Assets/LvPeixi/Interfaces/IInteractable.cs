
public interface IInteractable 
{
    string InteractObjectType { get; }
    /// <summary>
    /// Player与物体接触时此方法
    /// </summary>
    void StartContact();
    /// <summary>
    /// Player与物体结束触发此方法
    /// </summary>
    void EndContact();
    /// <summary>
    /// 玩家与物体开始互动时
    /// </summary>
    void StartInteract();
    /// <summary>
    /// 玩家和物体结束互动时
    /// </summary>
    /// <param name="result">互动</param>
    void EndInteract(object result);
    /// <summary>
    /// 显示互动图标
    /// </summary>
    /// <param name="result">互动</param>
    void ShowIcon();
    /// <summary>
    /// 隐藏互动图标
    /// </summary>
    /// <param name="result">互动</param>
    void HideIcon();
}

