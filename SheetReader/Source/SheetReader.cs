using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Sheet {

    /// <summary>
    /// 資料索引型態
    /// </summary>
    using pkey = Int64;

    /// <summary>
    /// 表格資料讀取器
    /// </summary>
    /// <typeparam name="T">表格資料型態</typeparam>
    public class Reader<T> where T : class {

        /// <summary>
        /// 接口: 取得資料索引
        /// </summary>
        /// <param name="data_">表格資料</param>
        /// <returns>資料索引</returns>
        public delegate pkey DelegatePkey(object data_);

        public Reader(DelegatePkey delegatePkey_) {
            delegatePkey = delegatePkey_;
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        /// <param name="jsons_">json字串列表</param>
        /// <returns>true表示成功, false則否</returns>
        public bool Set(string[] jsons_) {
            if (jsons_ == null)
                return false;

            foreach (var itor in jsons_) {
                if (Set(itor) == false)
                    return false;
            }//if

            return true;
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        /// <param name="json_">json字串</param>
        /// <returns>true表示成功, false則否</returns>
        public bool Set(string json_) {
            try {
                return Set(JsonConvert.DeserializeObject<T>(json_));
            } catch (Exception) {
                return false;
            }//try
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        /// <param name="data_">資料物件</param>
        /// <returns>true表示成功, false則否</returns>
        public bool Set(T data_) {
            if (data_ == null)
                return false;

            if (delegatePkey == null)
                return false;

            vaults[delegatePkey(data_)] = data_;

            return true;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="pkey_">資料索引</param>
        /// <returns>資料物件</returns>
        public T Get(pkey pkey_) {
            if (vaults.TryGetValue(pkey_, out var result))
                return result;

            return null;
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
        /// 取得資料索引列表
        /// </summary>
        /// <returns>資料索引列表</returns>
        public Dictionary<pkey, T>.KeyCollection Keys() {
            return vaults.Keys;
        }

        /// <summary>
        /// 取得資料物件列表
        /// </summary>
        /// <returns>資料物件列表</returns>
        public Dictionary<pkey, T>.ValueCollection Values() {
            return vaults.Values;
        }

        /// <summary>
        /// 取得資料索引與資料物件列表
        /// </summary>
        /// <returns>資料索引與資料物件列表</returns>
        public IEnumerator<KeyValuePair<pkey, T>> GetEnumerator() {
            return vaults.GetEnumerator();
        }

        /// <summary>
        /// 接口: 取得資料索引
        /// </summary>
        private DelegatePkey delegatePkey = null;

        /// <summary>
        /// 資料列表
        /// </summary>
        private Dictionary<pkey, T> vaults = new Dictionary<pkey, T>();
    }
}