using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

namespace Peixi
{
    public class IslandGridModule_model
    {
        public ReactiveDictionary<Vector2Int, IslandGridData> reactiveGridData = new ReactiveDictionary<Vector2Int, IslandGridData>();
        public Dictionary<Vector2Int, IslandGridData> gridData = new Dictionary<Vector2Int, IslandGridData>();
    }
    public struct IslandGridData
    {
        public IslandGridData(Vector2Int pos,
            bool hasIsland,
            string islandType = "Soil")
        {
            m_hasIsland = hasIsland;
            m_pos = pos;
            m_islandType = islandType;
        }
        public Vector2Int m_pos;
        public bool m_hasIsland;
        public string m_islandType;
    }
}