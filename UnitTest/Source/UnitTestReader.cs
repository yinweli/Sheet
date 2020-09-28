using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTest {

    [TestClass]
    public class UnitTestReader {

        private class Helper {

            public class Data {
                public long pkey;
                public int value1;
                public double value2;
                public string value3;
            }

            public static Data GetData(long pkey_ = 0) {
                return new Data() {
                    pkey = pkey_,
                    value1 = 100,
                    value2 = 22.2f,
                    value3 = "value3",
                };
            }

            public static string GetJson(long pkey_ = 0) {
                return JsonConvert.SerializeObject(GetData(pkey_));
            }

            public static string[] GetJsons() {
                return new string[] {
                    GetJson(0),
                    GetJson(1),
                    GetJson(2),
                };
            }

            public static long ToPkey(object data_) {
                return (data_ as Data).pkey;
            }
        }

        [TestMethod]
        public void SetJsons() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set(Helper.GetJsons());

            Assert.IsTrue(resultId == Sheet.ResultId.Success);
        }

        [TestMethod]
        public void SetJsonsNull() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set((string[])null);

            Assert.IsTrue(resultId == Sheet.ResultId.JsonNull);
        }

        [TestMethod]
        public void SetJson() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set(Helper.GetJson());

            Assert.IsTrue(resultId == Sheet.ResultId.Success);
        }

        [TestMethod]
        public void SetJsonDeserializeFailed() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set("??????????");

            Assert.IsTrue(resultId == Sheet.ResultId.DeserializeFailed);
        }

        [TestMethod]
        public void SetObject() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set(Helper.GetData());

            Assert.IsTrue(resultId == Sheet.ResultId.Success);
        }

        [TestMethod]
        public void SetObjectDataNull() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set((Helper.Data)null);

            Assert.IsTrue(resultId == Sheet.ResultId.DataNull);
        }

        [TestMethod]
        public void SetObjectDelegatePkeyNull() {
            var reader = new Sheet.Reader<Helper.Data>(null);
            var resultId = reader.Set(Helper.GetData());

            Assert.IsTrue(resultId == Sheet.ResultId.DelegatePkeyNull);
        }

        [TestMethod]
        public void Get() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set(Helper.GetData());

            Assert.IsTrue(resultId == Sheet.ResultId.Success);

            var data1 = Helper.GetData();
            var data2 = reader.Get(data1.pkey);

            Assert.IsTrue(data2 != null);
            Assert.IsTrue(data2.pkey == data1.pkey);
            Assert.IsTrue(data2.value1 == data1.value1);
            Assert.IsTrue(data2.value2 == data1.value2);
            Assert.IsTrue(data2.value3 == data1.value3);
        }

        [TestMethod]
        public void Clear() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set(Helper.GetData());

            Assert.IsTrue(resultId == Sheet.ResultId.Success);
            Assert.IsTrue(reader.Get(Helper.GetData().pkey) != null);

            reader.Clear();

            Assert.IsTrue(reader.Get(Helper.GetData().pkey) == null);
        }

        [TestMethod]
        public void Count() {
            var reader = new Sheet.Reader<Helper.Data>(Helper.ToPkey);
            var resultId = reader.Set(Helper.GetData());

            Assert.IsTrue(resultId == Sheet.ResultId.Success);
            Assert.IsTrue(reader.Count() == 1);
        }
    }
}