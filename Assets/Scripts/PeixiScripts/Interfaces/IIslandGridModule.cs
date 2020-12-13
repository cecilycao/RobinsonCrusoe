using System;
using UnityEngine;
using UniRx;

namespace Peixi
{
    public interface IIslandGridModule
    {
        void BuildIslandAt(Vector2Int buildPos);
        void RemoveIslandAt(Vector2Int buildPos);
        bool CheckThePositionHasIsland(Vector2Int buildPos);
        IObservable<DictionaryAddEvent<Vector2Int, IslandGridData>> OnIslandBuilt { get; }
        IObservable<DictionaryRemoveEvent<Vector2Int, IslandGridData>> OnIslandRemoved { get; }
    }
}
