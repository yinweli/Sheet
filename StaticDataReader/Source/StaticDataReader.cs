using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StaticData {

    /// <summary>
    /// ��Ư��ޫ��A
    /// </summary>
    using pkey = Int64;

    /// <summary>
    /// ���~�C�|
    /// </summary>
    public enum ErrorId {

        /// <summary>
        /// �ɦW����
        /// </summary>
        FileNameNull,

        /// <summary>
        /// �ɦW����
        /// </summary>
        FileNameEmpty,

        /// <summary>
        /// Ū���R�A��Ʊ��f����
        /// </summary>
        DelegateLoadNull,

        /// <summary>
        /// ���o��Ư��ޱ��f����
        /// </summary>
        DelegatePKeyNull,

        /// <summary>
        /// �ϧǦC�ƥ���
        /// </summary>
        DeserializeFailed,

        /// <summary>
        /// ��Ư��ޭ���
        /// </summary>
        PkeyDuplicate,

        /// <summary>
        /// ��Ư��޿��~
        /// </summary>
        PkeyFailed,
    }

    /// <summary>
    /// ���G���
    /// </summary>
    public struct Result {

        /// <summary>
        /// ���~�C�|
        /// </summary>
        public ErrorId error;

        /// <summary>
        /// �渹
        /// </summary>
        public int line;
    }

    /// <summary>
    /// �R�A���Ū����
    /// </summary>
    /// <typeparam name="T">�R�A��ƫ��A</typeparam>
    public class Reader<T> {

        /// <summary>
        /// ���f: Ū���R�A���
        /// </summary>
        /// <param name="filename_">����ɦW</param>
        /// <returns>�r��C��</returns>
        public delegate string[] DelegateLoad(string filename_);

        /// <summary>
        /// ���f: ���o��Ư���
        /// </summary>
        /// <param name="data_">�R�A���</param>
        /// <returns>��Ư���</returns>
        public delegate pkey DelegatePkey(object data_);

        public Reader(string filename_, DelegateLoad delegateLoad_, DelegatePkey delegatePkey_) {
            filename = filename_;
            delegateLoad = delegateLoad_;
            delegatePkey = delegatePkey_;
        }

        /// <summary>
        /// ��l�ƳB�z
        /// </summary>
        /// <returns>���G�C��</returns>
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
        /// ���o����ɦW
        /// </summary>
        /// <returns>����ɦW</returns>
        public string GetFileName() {
            return filename;
        }

        /// <summary>
        /// ���o���
        /// </summary>
        /// <param name="pkey_">��Ư���</param>
        /// <returns>���</returns>
        public T Get(pkey pkey_) {
            if (vaults.TryGetValue(pkey_, out var result))
                return result;
            else
                return default(T);
        }

        /// <summary>
        /// �]�w���
        /// </summary>
        /// <param name="data_">��ƪ���</param>
        /// <returns>true��ܦ��\, false�h�_</returns>
        public bool Set(T data_) {
            try {
                vaults[delegatePkey(data_)] = data_;

                return true;
            } catch (Exception) {
                return false;
            }//try
        }

        /// <summary>
        /// �M�����
        /// </summary>
        public void Clear() {
            vaults.Clear();
        }

        /// <summary>
        /// ���o��Ƽƶq
        /// </summary>
        /// <returns>��Ƽƶq</returns>
        public int Count() {
            return vaults.Count;
        }

        /// <summary>
        /// ���o��Ư��ަC��
        /// </summary>
        /// <returns>��Ư��ަC��</returns>
        public Dictionary<pkey, T>.KeyCollection Keys() {
            return vaults.Keys;
        }

        /// <summary>
        /// ���o��ƪ���C��
        /// </summary>
        /// <returns>��ƪ���C��</returns>
        public Dictionary<pkey, T>.ValueCollection Values() {
            return vaults.Values;
        }

        /// <summary>
        /// ���o��Ư��޻P��ƪ���C��
        /// </summary>
        /// <returns>��Ư��޻P��ƪ���C��</returns>
        public IEnumerator<KeyValuePair<pkey, T>> GetEnumerator() {
            return vaults.GetEnumerator();
        }

        /// <summary>
        /// ����ɦW
        /// </summary>
        private string filename = string.Empty;

        /// <summary>
        /// ���f: Ū���R�A���
        /// </summary>
        private DelegateLoad delegateLoad = null;

        /// <summary>
        /// ���f: ���o��Ư���
        /// </summary>
        private DelegatePkey delegatePkey = null;

        /// <summary>
        /// ��ƦC��
        /// </summary>
        private Dictionary<pkey, T> vaults = new Dictionary<pkey, T>();
    }
}