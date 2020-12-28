using Newtonsoft.Json;
using System;
using System.Collections.Generic;

#if UNITY_STANDALONE
using Sirenix.OdinInspector;
#endif

namespace SheetDefine {

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
        public delegate int DelegatePkey(T data_);

        public Reader(DelegatePkey delegatePkey_) {
            delegatePkey = delegatePkey_;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="pkey_">資料索引</param>
        /// <returns>資料物件</returns>
        public T Get(int pkey_) {
            if (vaults.TryGetValue(pkey_, out var result))
                return result;

            return null;
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

            try {
                vaults[delegatePkey(data_)] = data_;
            } catch (Exception) {
                return false;
            }//try

            return true;
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
        public Dictionary<int, T>.KeyCollection Keys() {
            return vaults.Keys;
        }

        /// <summary>
        /// 取得資料物件列表
        /// </summary>
        /// <returns>資料物件列表</returns>
        public Dictionary<int, T>.ValueCollection Values() {
            return vaults.Values;
        }

        /// <summary>
        /// 取得資料索引與資料物件列表
        /// </summary>
        /// <returns>資料索引與資料物件列表</returns>
        public IEnumerator<KeyValuePair<int, T>> GetEnumerator() {
            return vaults.GetEnumerator();
        }

        /// <summary>
        /// 接口: 取得資料索引
        /// </summary>
        private DelegatePkey delegatePkey = null;

        /// <summary>
        /// 表格資料列表
        /// </summary>
#if UNITY_STANDALONE
        [ShowInInspector, ReadOnly]
#endif
        private Dictionary<int, T> vaults = new Dictionary<int, T>();
    }

    /// <summary>
    /// 表格資料讀取器
    /// </summary>
    /// <typeparam name="T">原始表格資料型態</typeparam>
    /// <typeparam name="D">目標表格資料型態</typeparam>
    public class Reader<T, D> where T : class where D : class {

        /// <summary>
        /// 接口: 轉換表格資料
        /// </summary>
        /// <param name="data_">原始表格資料</param>
        /// <returns>目標表格資料</returns>
        public delegate D DelegateTranslate(T data_);

        /// <summary>
        /// 接口: 取得資料索引
        /// </summary>
        /// <param name="data_">目標表格資料</param>
        /// <returns>資料索引</returns>
        public delegate int DelegatePkey(D data_);

        public Reader(DelegateTranslate delegateTranslate_, DelegatePkey delegatePkey_) {
            delegateTranslate = delegateTranslate_;
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

            if (delegateTranslate == null)
                return false;

            if (delegatePkey == null)
                return false;

            try {
                var data = delegateTranslate(data_);
                var pkey = delegatePkey(data);

                vaults[pkey] = data;
            } catch (Exception) {
                return false;
            }//try

            return true;
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        /// <param name="data_">資料物件</param>
        /// <returns>true表示成功, false則否</returns>
        public bool Set(D data_) {
            if (data_ == null)
                return false;

            if (delegatePkey == null)
                return false;

            try {
                var pkey = delegatePkey(data_);

                vaults[pkey] = data_;
            } catch (Exception) {
                return false;
            }//try

            return true;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="pkey_">資料索引</param>
        /// <returns>資料物件</returns>
        public D Get(int pkey_) {
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
        public Dictionary<int, D>.KeyCollection Keys() {
            return vaults.Keys;
        }

        /// <summary>
        /// 取得資料物件列表
        /// </summary>
        /// <returns>資料物件列表</returns>
        public Dictionary<int, D>.ValueCollection Values() {
            return vaults.Values;
        }

        /// <summary>
        /// 取得資料索引與資料物件列表
        /// </summary>
        /// <returns>資料索引與資料物件列表</returns>
        public IEnumerator<KeyValuePair<int, D>> GetEnumerator() {
            return vaults.GetEnumerator();
        }

        /// <summary>
        /// 接口: 轉換表格資料
        /// </summary>
        private DelegateTranslate delegateTranslate = null;

        /// <summary>
        /// 接口: 取得資料索引
        /// </summary>
        private DelegatePkey delegatePkey = null;

        /// <summary>
        /// 表格資料列表
        /// </summary>
#if UNITY_STANDALONE
        [ShowInInspector, ReadOnly]
#endif
        private Dictionary<int, D> vaults = new Dictionary<int, D>();
    }
}