using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
                throw new InitializeException<T>("filename empty");

            if (delegateLoad == null)
                throw new InitializeException<T>("delegate load null");

            if (delegatePkey == null)
                throw new InitializeException<T>("delegate pkey null");

            vaults.Clear();

            var datas = delegateLoad(filename);
            var exceptions = new Dictionary<int, string>();

            for (var itor = 0; itor < datas.Count; ++itor) {
                var data = JsonConvert.DeserializeObject<T>(datas[itor]);

                if (data == null) {
                    exceptions.Add(itor, "deserialize failed");
                    continue;
                }//if

                var pkey = delegatePkey(data);

                try {
                    vaults.Add(pkey, data);
                }
                catch (ArgumentException) {
                    exceptions.Add(itor, "duplicate pkey");
                }
            }//for

            if (exceptions.Count > 0)
                throw new InitializeException<T>(exceptions);
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
    /// 初始化異常
    /// </summary>
    /// <typeparam name="T">靜態資料型態</typeparam>
    public class InitializeException<T> : Exception
    {
        /// <summary>
        /// 取得錯誤訊息
        /// </summary>
        /// <param name="message">錯誤訊息</param>
        /// <returns>錯誤訊息</returns>
        private static string GetErrorMessage(string message) {
            StringBuilder error = new StringBuilder();

            error.Append("{ ");
            error.Append("type=" + typeof(T).Name + ", ");
            error.Append("error=" + message + ", ");
            error.Append(" }");

            return error.ToString();
        }

        /// <summary>
        /// 取得錯誤訊息
        /// </summary>
        /// <param name="messages">錯誤列表</param>
        /// <returns>錯誤訊息</returns>
        private static string GetErrorMessage(Dictionary<int, string> messages) {
            StringBuilder error = new StringBuilder();

            error.Append("{ ");
            error.Append("type=" + typeof(T).Name + ", ");

            foreach (var itor in messages)
                error.Append("error=[line " + itor.Key + "] " + itor.Value + ", ");

            error.Append(" }");

            return error.ToString();
        }

        public InitializeException() {
        }

        /// <param name="message">錯誤訊息</param>
        public InitializeException(string message)
            : base(GetErrorMessage(message)) {
        }

        /// <param name="messages">錯誤列表</param>
        public InitializeException(Dictionary<int, string> messages)
            : base(GetErrorMessage(messages)) {
        }

        /// <param name="message">錯誤訊息</param>
        /// <param name="innerException">內部異常</param>
        public InitializeException(string message, Exception innerException)
            : base(GetErrorMessage(message), innerException) {
        }
    }
}