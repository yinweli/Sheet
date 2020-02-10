using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaticData;
using System;
using System.Collections.Generic;
using System.IO;

namespace UnitTestReaderCs
{
    [TestClass]
    public class UnitTest
    {
        [TestMethod]
        public void Test() {
            var reader = new Reader<TerrainData>() { filename = TerrainData.filename, delegateLoad = LoadJson, delegatePkey = ToPKey };

            try {
                reader.Initialize();
            }
            catch (Exception e) {
                Assert.Fail(e.ToString());
            }

            var data = reader.Get(1);

            Assert.IsNotNull(data);
            Assert.AreEqual(data.terrainType, 2);
            Assert.AreEqual(data.icon, "base00");
            Assert.AreEqual(data.sprite, "Map/Base/base00");
            CollectionAssert.AreEqual(data.testReal, new List<double>() { 0.1, 0.2, 0.3, 0.4, 0.5, 1.1, 1.2, 10.5, 200.1, 300.99 });
            CollectionAssert.AreEqual(data.testText, new List<string>() { "a", "ab", "abc", "字串", "嘿嘿嘿", "測試字串很多字喔" });
        }

        [TestMethod]
        public void TestFilenameEmpty() {
            var reader = new Reader<TerrainData>() { filename = null, delegateLoad = LoadJson, delegatePkey = ToPKey };
            var result = false;

            try {
                reader.Initialize();
            }
            catch (ExceptionFileName e) {
                result = true;
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDelegateLoadNull() {
            var reader = new Reader<TerrainData>() { filename = TerrainData.filename, delegateLoad = null, delegatePkey = ToPKey };
            var result = false;

            try {
                reader.Initialize();
            }
            catch (ExceptionDelegateLoad e) {
                result = true;
            }

            Assert.IsTrue(result);
        }

        [TestMethod]
        public void TestDelegatePKeyNull() {
            var reader = new Reader<TerrainData>() { filename = TerrainData.filename, delegateLoad = LoadJson, delegatePkey = null };
            var result = false;

            try {
                reader.Initialize();
            }
            catch (ExceptionDelegatePKey e) {
                result = true;
            }

            Assert.IsTrue(result);
        }

        public TestContext TestContext {
            get; set;
        }

        private List<string> LoadJson(string filename) {
            var filepath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "_Output", "Json", filename);
            var file = File.ReadAllLines(filepath);

            return new List<string>(file);
        }

        private long ToPKey(object data) {
            return (data as TerrainData)?.terrainId ?? 0;
        }
    }
}