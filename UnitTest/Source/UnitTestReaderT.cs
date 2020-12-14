using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTest {

    [TestClass]
    public class UnitTestReaderT {

        private class Helper {

            public class Data {
                public int pkey;
                public bool value0;
                public int value1;
                public double value2;
                public string value3;
            }

            public static Data GetData(int pkey_ = 0) {
                return new Data() {
                    pkey = pkey_,
                    value0 = true,
                    value1 = 100,
                    value2 = 22.2f,
                    value3 = "value3",
                };
            }

            public static string GetJson(int pkey_ = 0) {
                return JsonConvert.SerializeObject(GetData(pkey_));
            }

            public static string[] GetJsons() {
                return new string[] {
                    GetJson(0),
                    GetJson(1),
                    GetJson(2),
                };
            }

            public static int ToPkey(Data data_) {
                return data_.pkey;
            }
        }

        [TestMethod]
        public void SetJsons() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetJsons()));
        }

        [TestMethod]
        public void SetJsonsNull() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set((string[])null) == false);
        }

        [TestMethod]
        public void SetJson() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetJson()));
        }

        [TestMethod]
        public void SetJsonDeserializeFailed() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set("??????????") == false);
        }

        [TestMethod]
        public void SetData() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetData()));
        }

        [TestMethod]
        public void SetDataDataNull() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set((Helper.Data)null) == false);
        }

        [TestMethod]
        public void SetDataDelegatePkeyNull() {
            var reader = new SheetDefine.Reader<Helper.Data>(null);

            Assert.IsTrue(reader.Set(Helper.GetData()) == false);
        }

        [TestMethod]
        public void Get() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetData()));

            var data1 = Helper.GetData();
            var data2 = reader.Get(data1.pkey);

            Assert.IsTrue(data2 != null);
            Assert.IsTrue(data2.pkey == data1.pkey);
            Assert.IsTrue(data2.value0 == data1.value0);
            Assert.IsTrue(data2.value1 == data1.value1);
            Assert.IsTrue(data2.value2 == data1.value2);
            Assert.IsTrue(data2.value3 == data1.value3);
        }

        [TestMethod]
        public void Clear() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetData()));
            Assert.IsTrue(reader.Get(Helper.GetData().pkey) != null);

            reader.Clear();

            Assert.IsTrue(reader.Get(Helper.GetData().pkey) == null);
        }

        [TestMethod]
        public void Count() {
            var reader = new SheetDefine.Reader<Helper.Data>(Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetData()));
            Assert.IsTrue(reader.Count() == 1);
        }
    }
}