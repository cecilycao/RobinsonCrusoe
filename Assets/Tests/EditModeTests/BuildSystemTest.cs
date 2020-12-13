using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Peixi;
using UniRx;

namespace Tests
{
    public class BuildSystemTest
    {
        // A Test behaves as an ordinary method
        [Test]
        public void IslandGridModule_CheckThePositionHasIsland_x0y0_true()
        {
            IIslandGridModule grid = new IslandGridModulePresenter();
            grid.BuildIslandAt(Vector2Int.zero);
            var hasIsland = grid.CheckThePositionHasIsland(Vector2Int.zero);
            Assert.IsTrue(hasIsland);
        }
        [Test]
        public void IslandGridModule_RemoveIslandAt_x2y1_false()
        {
            IIslandGridModule grid = new IslandGridModulePresenter();
            var pos = new Vector2Int(2, 1);
            grid.BuildIslandAt(pos);
            var hasIslandAtx2y1 = grid.CheckThePositionHasIsland(pos);
            Assert.IsTrue(hasIslandAtx2y1);
            grid.RemoveIslandAt(pos);
            hasIslandAtx2y1 = grid.CheckThePositionHasIsland(pos);
            Assert.IsFalse(hasIslandAtx2y1);
        }
        [Test]
        public void IslandGridModule_OnIslandBuilt_true()
        {
            IIslandGridModule grid = GameObject.FindObjectOfType<IslandGridModulePresenter>();
            var hasIslandAtx2y3 = false;
            grid.OnIslandBuilt
                .Subscribe(x =>
                {
                    hasIslandAtx2y3 = x.Value.m_hasIsland;
                });
            grid.BuildIslandAt(new Vector2Int(2, 3));
            Assert.IsTrue(hasIslandAtx2y3);
        }
        [Test]
        public void IslandGridModule_OnIslandRemoved_false()
        {
            IIslandGridModule grid = GameObject.FindObjectOfType<IslandGridModulePresenter>();
            var pos = new Vector2Int(1, 3);
            grid.BuildIslandAt(pos);
            grid.RemoveIslandAt(pos);
            var hasIslandAtx1y3 = grid.CheckThePositionHasIsland(pos);
            Assert.IsFalse(hasIslandAtx1y3);
        }

        // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
        // `yield return null;` to skip a frame.
        [UnityTest]
        public IEnumerator BuildSystemTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return null;
        }
    }
}
