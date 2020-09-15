using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StaticData {

    /// <summary>
    /// 資料索引型態
    /// </summary>
    using pkey = Int64;

    /// <summary>
    /// 錯誤列舉
    /// </summary>
    public enum ErrorId {

        /// <summary>
        /// 檔名為空
        /// </summary>
        FileNameNull,

        /// <summary>
        /// 檔名為空
        /// </summary>
        FileNameEmpty,

        /// <summary>
        /// 讀取靜態資料接口為空
        /// </summary>
        DelegateLoadNull,

        /// <summary>
        /// 取得資料索引接口為空
        /// </summary>
        DelegatePKeyNull,

        /// <summary>
        /// 反序列化失敗
        /// </summary>
        DeserializeFailed,

        /// <summary>
        /// 資料索引重複
        /// </summary>
        PkeyDuplicate,

        /// <summary>
        /// 資料索引錯誤
        /// </summary>
        PkeyFailed,
    }

    /// <summary>
    /// 結果資料
    /// </summary>
    public struct Result {

        /// <summary>
        /// 錯誤列舉
        /// </summary>
        public ErrorId error;

        /// <summary>
        /// 行號
        /// </summary>
        public int line;
    }

    /// <summary>
    /// 靜態資料讀取器
    /// </summary>
    /// <typeparam name="T">靜態資料型態</typeparam>
    public class Reader<T> {

        /// <summary>
        /// 接口: 讀取靜態資料
        /// </summary>
        /// <param name="filename_">資料檔名</param>
        /// <returns>字串列表</returns>
        public delegate string[] DelegateLoad(string filename_);

        /// <summary>
        /// 接口: 取得資料索引
        /// </summary>
        /// <param name="data_">靜態資料</param>
        /// <returns>資料索引</returns>
        public delegate pkey DelegatePkey(object data_);

        public Reader(string filename_, DelegateLoad delegateLoad_, DelegatePkey delegatePkey_) {
            filename = filename_;
            delegateLoad = delegateLoad_;
            delegatePkey = delegatePkey_;
        }

        /// <summary>
        /// 初始化處理
        /// </summary>
        /// <returns>結果列表</returns>
        public Result[] Initialize() {
            if (filename == null)
                return new Result[] { new Result() { error = ErrorId.FileNameNull } };

            if (filename.Length <= 0)
                return new Result[] { new Result() { error = ErrorId.FileNameEmpty } };

            if (delegateLoad == null)
                return new Result[] { new Result() { error = ErrorId.DelegateLoadNull } };

            if (delegatePkey == null)
                return new Result[] { new Result() { error = ErrorId.DelegatePKeyNull } };

            vaults.Clear();

            var datas = delegateLoad(filename);
            var results = new List<Result>();

            for (var itor = 0; itor < datas.Length; ++itor) {
                T data = default;

                try {
                    data = JsonConvert.DeserializeObject<T>(datas[itor]);
                } catch (Exception) {
                    results.Add(new Result() { error = ErrorId.DeserializeFailed, line = itor });
                    continue;
                }//try

                try {
                    vaults.Add(delegatePkey(data), data);
                } catch (ArgumentException) {
                    results.Add(new Result() { error = ErrorId.PkeyDuplicate, line = itor });
                } catch (Exception) {
                    results.Add(new Result() { error = ErrorId.PkeyFailed, line = itor });
                }//try
            }//for

            return results.ToArray();
        }

        /// <summary>
        /// 取得資料檔名
        /// </summary>
        /// <returns>資料檔名</returns>
        public string GetFileName() {
            return filename;
        }

        /// <summary>
        /// 取得資料
        /// </summary>
        /// <param name="pkey_">資料索引</param>
        /// <returns>資料</returns>
        public T Get(pkey pkey_) {
            if (vaults.TryGetValue(pkey_, out var result))
                return result;
            else
                return default(T);
        }

        /// <summary>
        /// 設定資料
        /// </summary>
        /// <param name="data_">資料物件</param>
        /// <returns>true表示成功, false則否</returns>
        public bool Set(T data_) {
            try {
                vaults[delegatePkey(data_)] = data_;

                return true;
            } catch (Exception) {
                return false;
            }//try
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
        /// 資料檔名
        /// </summary>
        private string filename = string.Empty;

        /// <summary>
        /// 接口: 讀取靜態資料
        /// </summary>
        private DelegateLoad delegateLoad = null;

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