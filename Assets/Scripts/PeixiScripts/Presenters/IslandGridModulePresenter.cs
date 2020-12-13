using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using System;

namespace Peixi
{
    public class IslandGridModulePresenter : MonoBehaviour, IIslandGridModule
    {
        protected IslandGridModule_model model = new IslandGridModule_model();
        private ReactiveCollection<string> testReactiveCollection = new ReactiveCollection<string>();
        public IObservable<DictionaryAddEvent<Vector2Int, IslandGridData>> OnIslandBuilt => model.reactiveGridData.ObserveAdd();
        public IObservable<DictionaryRemoveEvent<Vector2Int, IslandGridData>> OnIslandRemoved => model.reactiveGridData.ObserveRemove();
        public void BuildIslandAt(Vector2Int buildPos)
        {
            var hasIsandAtThePosition = CheckThePositionHasIsland(buildPos);
            if (hasIsandAtThePosition)
            {
                throw new Exception("A island has been arranged at " + buildPos + " previously");
            }
            model.reactiveGridData.Add(buildPos,
                new IslandGridData(buildPos, true));
        }
        public bool CheckThePositionHasIsland(Vector2Int buildPos)
        {
            var hasIsland = model.reactiveGridData.ContainsKey(buildPos);
            return hasIsland;
        }
        public void RemoveIslandAt(Vector2Int buildPos)
        {
            var hasIslandAtPos = CheckThePositionHasIsland(buildPos);
            if (!hasIslandAtPos)
            {
                throw new Exception("No island at the pointed pos: " + buildPos);
            }
            model.reactiveGridData.Remove(buildPos);
        }
    }
}

