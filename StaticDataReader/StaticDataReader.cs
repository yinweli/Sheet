using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StaticData
{
    /// <summary>
    /// 索引型態
    /// </summary>
    using pkey = Int64;

    /// <summary>
    /// 靜態資料讀取器
    /// </summary>
    /// <typeparam name="T">靜態資料型態</typeparam>
    public class Reader<T>
    {
        /// <summary>
        /// 接口: 讀取靜態資料
        /// </summary>
        /// <param name="filename">靜態資料檔名</param>
        /// <returns>靜態資料字串列表</returns>
        public delegate List<string> DelegateLoad(string filename);

        /// <summary>
        /// 接口: 取得靜態資料索引
        /// </summary>
        /// <param name="data">靜態資料</param>
        /// <returns>索引</returns>
        public delegate pkey DelegatePkey(object data);

        /// <summary>
        /// 初始化處理
        /// </summary>
        public void Initialize() {
            if (filename == null || filename.Length <= 0)
                throw new ExceptionFileName(typeof(T).Name);

            if (delegateLoad == null)
                throw new ExceptionDelegateLoad(typeof(T).Name);

            if (delegatePkey == null)
                throw new ExceptionDelegatePKey(typeof(T).Name);

            vaults.Clear();

            var datas = delegateLoad(filename);
            var errors = new Dictionary<int, string>();

            for (var itor = 0; itor < datas.Count; ++itor) {
                var data = JsonConvert.DeserializeObject<T>(datas[itor]);

                if (data == null) {
                    errors.Add(itor, "deserialize failed");
                    continue;
                }//if

                var pkey = delegatePkey(data);

                try {
                    vaults.Add(pkey, data);
                }
                catch (ArgumentException) {
                    errors.Add(itor, "duplicate pkey");
                }
            }//for

            if (errors.Count > 0)
                throw new ExceptionData(typeof(T).Name, errors);
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="pkey">索引</param>
        /// <returns>資料</returns>
        public T Get(pkey pkey) {
            if (vaults.TryGetValue(pkey, out T result))
                return result;
            else
                return default(T);
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        /// <param name="pkey">索引</param>
        /// <param name="data">資料</param>
        public void Set(pkey pkey, T data) {
            vaults[pkey] = data;
        }

        /// <summary>
        /// 清除資料
        /// </summary>
        public void Clear() {
            vaults.Clear();
        }

        /// <summary>
        /// 取得資料數量
        /// </summary>
        /// <returns>資料數量</returns>
        public int Count() {
            return vaults.Count;
        }

        /// <summary>
        /// 取得索引列表
        /// </summary>
        /// <returns>索引列表</returns>
        public Dictionary<pkey, T>.KeyCollection Keys() {
            return vaults.Keys;
        }

        /// <summary>
        /// 取得資料列表
        /// </summary>
        /// <returns>資料列表</returns>
        public Dictionary<pkey, T>.ValueCollection Values() {
            return vaults.Values;
        }

        /// <summary>
        /// 取得索引資料列表
        /// </summary>
        /// <returns>索引資料列表</returns>
        public IEnumerator<KeyValuePair<pkey, T>> GetEnumerator() {
            return vaults.GetEnumerator();
        }

        /// <summary>
        /// 靜態資料檔名
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// 接口: 讀取靜態資料
        /// </summary>
        public DelegateLoad delegateLoad { get; set; }

        /// <summary>
        /// 接口: 取得靜態資料索引
        /// </summary>
        public DelegatePkey delegatePkey { get; set; }

        /// <summary>
        /// 資料列表
        /// </summary>
        private Dictionary<pkey, T> vaults = new Dictionary<pkey, T>();
    }

    /// <summary>
    /// 異常: 檔名為空
    /// </summary>
    public class ExceptionFileName : Exception
    {
        /// <param name="type">類型字串</param>
        public ExceptionFileName(string type)
            : base("filename empty, type=" + type) {
        }
    }

    /// <summary>
    /// 異常: 讀取靜態資料接口為空
    /// </summary>
    public class ExceptionDelegateLoad : Exception
    {
        /// <param name="type">類型字串</param>
        public ExceptionDelegateLoad(string type)
            : base("delegate load null, type=" + type) {
        }
    }

    /// <summary>
    /// 異常: 取得靜態資料索引接口為空
    /// </summary>
    public class ExceptionDelegatePKey : Exception
    {
        /// <param name="type">類型字串</param>
        public ExceptionDelegatePKey(string type)
            : base("delegate pkey null, type=" + type) {
        }
    }

    /// <summary>
    /// 異常: 資料失敗
    /// </summary>
    public class ExceptionData : Exception
    {
        /// <param name="type">類型字串</param>
        /// <param name="errors">錯誤列表</param>
        public ExceptionData(string type, Dictionary<int, string> errors)
            : base("data failed, type=" + type + ", errors=" + errors) {
        }
    }
}