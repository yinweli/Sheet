using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace UnitTest {

    [TestClass]
    public class UnitTestReaderD {

        private class Helper {

            public class DataT {
                public int pkey;
                public bool value0;
                public int value1;
                public double value2;
                public string value3;
            }

            public class DataD {
                public int pkey;
                public bool value0;
                public int value1;
                public double value2;
                public string value3;
            }

            public static DataT GetDataT(int pkey_ = 0) {
                return new DataT() {
                    pkey = pkey_,
                    value0 = true,
                    value1 = 100,
                    value2 = 22.2f,
                    value3 = "value3",
                };
            }

            public static DataD GetDataD(int pkey_ = 0) {
                return new DataD() {
                    pkey = pkey_,
                    value0 = true,
                    value1 = 100,
                    value2 = 22.2f,
                    value3 = "value3",
                };
            }

            public static string GetJson(int pkey_ = 0) {
                return JsonConvert.SerializeObject(GetDataT(pkey_));
            }

            public static string[] GetJsons() {
                return new string[] {
                    GetJson(0),
                    GetJson(1),
                    GetJson(2),
                };
            }

            public static DataD ToData(DataT data_) {
                return new DataD() {
                    pkey = data_.pkey,
                    value0 = data_.value0,
                    value1 = data_.value1,
                    value2 = data_.value2,
                    value3 = data_.value3,
                };
            }

            public static int ToPkey(DataD data_) {
                return data_.pkey;
            }
        }

        [TestMethod]
        public void Get() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetDataD()));

            var data1 = Helper.GetDataD();
            var data2 = reader.Get(data1.pkey);

            Assert.IsTrue(data2 != null);
            Assert.IsTrue(data2.pkey == data1.pkey);
            Assert.IsTrue(data2.value0 == data1.value0);
            Assert.IsTrue(data2.value1 == data1.value1);
            Assert.IsTrue(data2.value2 == data1.value2);
            Assert.IsTrue(data2.value3 == data1.value3);
        }

        [TestMethod]
        public void SetJsons() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetJsons()));
        }

        [TestMethod]
        public void SetJsonsNull() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set((string[])null) == false);
        }

        [TestMethod]
        public void SetJson() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetJson()));
        }

        [TestMethod]
        public void SetJsonDeserializeFailed() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set("??????????") == false);
        }

        [TestMethod]
        public void SetDataT() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetDataT()));
        }

        [TestMethod]
        public void SetDataTDataNull() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set((Helper.DataT)null) == false);
        }

        [TestMethod]
        public void SetDataTDelegateTranslateNull() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(null, Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetDataT()) == false);
        }

        [TestMethod]
        public void SetDataTDelegatePkeyNull() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, null);

            Assert.IsTrue(reader.Set(Helper.GetDataT()) == false);
        }

        [TestMethod]
        public void SetDataD() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetDataD()));
        }

        [TestMethod]
        public void SetDataDDataNull() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set((Helper.DataD)null) == false);
        }

        [TestMethod]
        public void SetDataDDelegatePkeyNull() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, null);

            Assert.IsTrue(reader.Set(Helper.GetDataD()) == false);
        }

        [TestMethod]
        public void Clear() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetDataD()));
            Assert.IsTrue(reader.Get(Helper.GetDataD().pkey) != null);

            reader.Clear();

            Assert.IsTrue(reader.Get(Helper.GetDataD().pkey) == null);
        }

        [TestMethod]
        public void Count() {
            var reader = new SheetDefine.Reader<Helper.DataT, Helper.DataD>(Helper.ToData, Helper.ToPkey);

            Assert.IsTrue(reader.Set(Helper.GetDataD()));
            Assert.IsTrue(reader.Count() == 1);
        }
    }
}