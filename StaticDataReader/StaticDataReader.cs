using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace StaticData
{
    /// <summary>
    /// ���ޫ��A
    /// </summary>
    using pkey = Int64;

    /// <summary>
    /// �R�A���Ū����
    /// </summary>
    /// <typeparam name="T">�R�A��ƫ��A</typeparam>
    public class Reader<T>
    {
        /// <summary>
        /// ���f: Ū���R�A���
        /// </summary>
        /// <param name="filename">�R�A����ɦW</param>
        /// <returns>�R�A��Ʀr��C��</returns>
        public delegate List<string> DelegateLoad(string filename);

        /// <summary>
        /// ���f: ���o�R�A��Ư���
        /// </summary>
        /// <param name="data">�R�A���</param>
        /// <returns>����</returns>
        public delegate pkey DelegatePkey(object data);

        /// <summary>
        /// ��l�ƳB�z
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
        /// ���o���
        /// </summary>
        /// <param name="pkey">����</param>
        /// <returns>���</returns>
        public T Get(pkey pkey) {
            if (vaults.TryGetValue(pkey, out T result))
                return result;
            else
                return default(T);
        }

        /// <summary>
        /// �]�w���
        /// </summary>
        /// <param name="pkey">����</param>
        /// <param name="data">���</param>
        public void Set(pkey pkey, T data) {
            vaults[pkey] = data;
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
        /// ���o���ަC��
        /// </summary>
        /// <returns>���ަC��</returns>
        public Dictionary<pkey, T>.KeyCollection Keys() {
            return vaults.Keys;
        }

        /// <summary>
        /// ���o��ƦC��
        /// </summary>
        /// <returns>��ƦC��</returns>
        public Dictionary<pkey, T>.ValueCollection Values() {
            return vaults.Values;
        }

        /// <summary>
        /// ���o���޸�ƦC��
        /// </summary>
        /// <returns>���޸�ƦC��</returns>
        public IEnumerator<KeyValuePair<pkey, T>> GetEnumerator() {
            return vaults.GetEnumerator();
        }

        /// <summary>
        /// �R�A����ɦW
        /// </summary>
        public string filename { get; set; }

        /// <summary>
        /// ���f: Ū���R�A���
        /// </summary>
        public DelegateLoad delegateLoad { get; set; }

        /// <summary>
        /// ���f: ���o�R�A��Ư���
        /// </summary>
        public DelegatePkey delegatePkey { get; set; }

        /// <summary>
        /// ��ƦC��
        /// </summary>
        private Dictionary<pkey, T> vaults = new Dictionary<pkey, T>();
    }

    /// <summary>
    /// ���`: �ɦW����
    /// </summary>
    public class ExceptionFileName : Exception
    {
        /// <param name="type">�����r��</param>
        public ExceptionFileName(string type)
            : base("filename empty, type=" + type) {
        }
    }

    /// <summary>
    /// ���`: Ū���R�A��Ʊ��f����
    /// </summary>
    public class ExceptionDelegateLoad : Exception
    {
        /// <param name="type">�����r��</param>
        public ExceptionDelegateLoad(string type)
            : base("delegate load null, type=" + type) {
        }
    }

    /// <summary>
    /// ���`: ���o�R�A��Ư��ޱ��f����
    /// </summary>
    public class ExceptionDelegatePKey : Exception
    {
        /// <param name="type">�����r��</param>
        public ExceptionDelegatePKey(string type)
            : base("delegate pkey null, type=" + type) {
        }
    }

    /// <summary>
    /// ���`: ��ƥ���
    /// </summary>
    public class ExceptionData : Exception
    {
        /// <param name="type">�����r��</param>
        /// <param name="errors">���~�C��</param>
        public ExceptionData(string type, Dictionary<int, string> errors)
            : base("data failed, type=" + type + ", errors=" + errors) {
        }
    }
}