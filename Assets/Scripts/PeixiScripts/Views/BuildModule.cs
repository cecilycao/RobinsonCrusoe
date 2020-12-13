using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UniRx;

namespace Peixi
{
    public class BuildModule : MonoBehaviour
    {
        IIslandGridModule grid;

        public int gridSize = 5;
        public Vector3 gridOrigialPos = Vector3.zero;
        public Vector3 gridOffset = Vector3.zero;

        public int[] t = { 9, 6, 3 };
        protected Dictionary<Vector2Int, GameObject> islandSquares = new Dictionary<Vector2Int, GameObject>();
        private void OnEnable()
        {
            grid = GetComponent<IslandGridModulePresenter>();
        }
        private void Start()
        {
            grid.OnIslandBuilt
                .Subscribe(x =>
                {
                    CreateIslandInGrid(x.Key);
                });

            grid.OnIslandRemoved
                .Subscribe(x =>
                {
                    RemoveIslandAt(x.Key);
                });
        }
        protected void CreateIslandInGrid(Vector2Int gridPos)
        {
            GameObject island = GameObject.CreatePrimitive(PrimitiveType.Cube);
            island.transform.localScale = new Vector3(5, 1, 5);
            island.transform.name = "IslandAt" + gridPos.x + "dot" + gridPos.y;
            var islandPosInWorld = new Vector3(gridPos.x * gridSize, 0, gridPos.y * gridSize);
            island.transform.position = islandPosInWorld;
            islandSquares.Add(gridPos, island);
        }
        protected void RemoveIslandAt(Vector2Int pos)
        {
            GameObject.Destroy(islandSquares[pos]);
            islandSquares.Remove(pos);                                       
        }
    }
}
