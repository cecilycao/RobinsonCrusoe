using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Peixi;
using UniRx;
using System;

namespace Tests
{
    public class BuildSystemTestUnderPlayerMode
    {
        GameObject island1;
        GameObject island2;
        TestableBuildModule testObject = new TestableBuildModule();
        [SetUp]
        public void Setup()
        {
            island1 = testObject.BuildIsland(Vector2Int.zero);
            island2 = testObject.BuildIsland(new Vector2Int(1, 1));
        }
        [UnityTest]
        public IEnumerator BuildModule_OnIslandBuilt_NotNullAndTrue()
        {
            Assert.IsNotNull(island1);
            Assert.IsNotNull(island2);
            Assert.AreEqual(island1.transform.position, Vector3.zero);
            Assert.AreEqual(island2.transform.position, new Vector3(5,0,5));
            yield return null;
        }
        [UnityTest]
        public IEnumerator BuildModule_OnIslandRemoved_false()
        {
            testObject.RemoveIsland(Vector2Int.zero);
            var hasIsland = testObject.HasIslandAt(Vector2Int.zero);
            Assert.IsFalse(hasIsland);
            yield return null;
        }
    }
}
