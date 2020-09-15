using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Linq;

namespace UnitTest {

    [TestClass]
    public class UnitTestReader {

        public class FakeData {
            public const string filename = "filename";

            public static readonly FakeData data = new FakeData() {
                pkey = 1,
                value1 = 100,
                value2 = 22.2f,
                value3 = "value3",
                list1 = new int[] { 111, 222, 333 },
                list2 = new double[] { 44.4f, 55.5f, 66.6f },
                list3 = new string[] { "str1", "str2", "str3" },
            };

            public long pkey;
            public int value1;
            public double value2;
            public string value3;
            public int[] list1;
            public double[] list2;
            public string[] list3;
        }

        [TestMethod]
        public void Initialize() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length <= 0);
        }

        [TestMethod]
        public void InitializeFileNameNull() {
            var reader = new StaticData.Reader<FakeData>(null, LoadJson, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length == 1);
            Assert.IsTrue(results[0].error == StaticData.ErrorId.FileNameNull);
        }

        [TestMethod]
        public void InitializeFileNameEmpty() {
            var reader = new StaticData.Reader<FakeData>(string.Empty, LoadJson, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length == 1);
            Assert.IsTrue(results[0].error == StaticData.ErrorId.FileNameEmpty);
        }

        [TestMethod]
        public void InitializeDelegateLoadNull() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, null, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length == 1);
            Assert.IsTrue(results[0].error == StaticData.ErrorId.DelegateLoadNull);
        }

        [TestMethod]
        public void InitializeDelegatePKeyNull() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, null);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length == 1);
            Assert.IsTrue(results[0].error == StaticData.ErrorId.DelegatePKeyNull);
        }

        [TestMethod]
        public void InitializeDeserializeFailed() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJsonDeserializeFailed, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length == 1);
            Assert.IsTrue(results[0].error == StaticData.ErrorId.DeserializeFailed);
        }

        [TestMethod]
        public void InitializePkeyDuplicate() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJsonPkeyDuplicate, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length == 1);
            Assert.IsTrue(results[0].error == StaticData.ErrorId.PkeyDuplicate);
        }

        [TestMethod]
        public void InitializePkeyFailed() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, ToPKeyFailed);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length == 1);
            Assert.IsTrue(results[0].error == StaticData.ErrorId.PkeyFailed);
        }

        [TestMethod]
        public void GetFileName() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, ToPKey);

            Assert.IsTrue(reader.GetFileName() == FakeData.filename);
        }

        [TestMethod]
        public void Get() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length <= 0);

            var data1 = FakeData.data;
            var data2 = reader.Get(data1.pkey);

            Assert.IsTrue(data2 != null);
            Assert.IsTrue(data2.pkey == data1.pkey);
            Assert.IsTrue(data2.value1 == data1.value1);
            Assert.IsTrue(data2.value2 == data1.value2);
            Assert.IsTrue(data2.value3 == data1.value3);
            Assert.IsTrue(data2.list1.Except(data1.list1).ToArray().Length <= 0);
            Assert.IsTrue(data2.list2.Except(data1.list2).ToArray().Length <= 0);
            Assert.IsTrue(data2.list3.Except(data1.list3).ToArray().Length <= 0);
        }

        [TestMethod]
        public void Set() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length <= 0);
            Assert.IsTrue(reader.Set(FakeData.data));

            var data1 = FakeData.data;
            var data2 = reader.Get(data1.pkey);

            Assert.IsTrue(data2 != null);
            Assert.IsTrue(data2.pkey == data1.pkey);
            Assert.IsTrue(data2.value1 == data1.value1);
            Assert.IsTrue(data2.value2 == data1.value2);
            Assert.IsTrue(data2.value3 == data1.value3);
            Assert.IsTrue(data2.list1.Except(data1.list1).ToArray().Length <= 0);
            Assert.IsTrue(data2.list2.Except(data1.list2).ToArray().Length <= 0);
            Assert.IsTrue(data2.list3.Except(data1.list3).ToArray().Length <= 0);
        }

        [TestMethod]
        public void SetNull() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length <= 0);
            Assert.IsTrue(reader.Set(null) == false);
        }

        [TestMethod]
        public void Clear() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length <= 0);
            Assert.IsTrue(reader.Get(FakeData.data.pkey) != null);

            reader.Clear();

            Assert.IsTrue(reader.Get(FakeData.data.pkey) == null);
        }

        [TestMethod]
        public void Count() {
            var reader = new StaticData.Reader<FakeData>(FakeData.filename, LoadJson, ToPKey);
            var results = reader.Initialize();

            Assert.IsTrue(results.Length <= 0);
            Assert.IsTrue(reader.Count() == 1);
        }

        private string[] LoadJson(string filename_) {
            return new string[] { JsonConvert.SerializeObject(FakeData.data) };
        }

        private string[] LoadJsonDeserializeFailed(string filename_) {
            return new string[] { "??????????" };
        }

        private string[] LoadJsonPkeyDuplicate(string filename_) {
            var jsonString = JsonConvert.SerializeObject(FakeData.data);

            return new string[] { jsonString, jsonString };
        }

        private long ToPKey(object data_) {
            return (data_ as FakeData).pkey;
        }

        private long ToPKeyFailed(object data_) {
            throw new Exception();
        }
    }
}