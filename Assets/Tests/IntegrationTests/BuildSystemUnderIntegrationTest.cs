using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using IngameDebugConsole;
using Peixi;

public class BuildSystemUnderIntegrationTest : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        DebugLogConsole.AddCommand<Vector2Int>("buildIslandAt", "在网格坐标[x,y]处增加Island", BuildIslandAt);
        DebugLogConsole.AddCommand<Vector2Int>("removeIslandAt", "移除网格坐标[x,y]处的Island", RemoveIslandAt);
    }

    public static void BuildIslandAt(Vector2Int gridPos)
    {
        IIslandGridModule buildModule = FindObjectOfType<IslandGridModulePresenter>();
        buildModule.BuildIslandAt(gridPos);
    }

    public static void RemoveIslandAt(Vector2Int gridPos)
    {
        IIslandGridModule buildModule = FindObjectOfType<IslandGridModulePresenter>();
        buildModule.RemoveIslandAt(gridPos);
    }
}
