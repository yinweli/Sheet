using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

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
    /// ��l�Ʋ��`
    /// </summary>
    /// <typeparam name="T">�R�A��ƫ��A</typeparam>
    public class InitializeException<T> : Exception
    {
        /// <summary>
        /// ���o���~�T��
        /// </summary>
        /// <param name="message">���~�T��</param>
        /// <returns>���~�T��</returns>
        private static string GetErrorMessage(string message) {
            StringBuilder error = new StringBuilder();

            error.Append("{ ");
            error.Append("type=" + typeof(T).Name + ", ");
            error.Append("error=" + message + ", ");
            error.Append(" }");

            return error.ToString();
        }

        /// <summary>
        /// ���o���~�T��
        /// </summary>
        /// <param name="messages">���~�C��</param>
        /// <returns>���~�T��</returns>
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

        /// <param name="message">���~�T��</param>
        public InitializeException(string message)
            : base(GetErrorMessage(message)) {
        }

        /// <param name="messages">���~�C��</param>
        public InitializeException(Dictionary<int, string> messages)
            : base(GetErrorMessage(messages)) {
        }

        /// <param name="message">���~�T��</param>
        /// <param name="innerException">�������`</param>
        public InitializeException(string message, Exception innerException)
            : base(GetErrorMessage(message), innerException) {
        }
    }
}