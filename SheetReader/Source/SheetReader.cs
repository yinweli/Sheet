using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Sheet {

    /// <summary>
    /// ��Ư��ޫ��A
    /// </summary>
    using pkey = Int64;

    /// <summary>
    /// �����Ū����
    /// </summary>
    /// <typeparam name="T">����ƫ��A</typeparam>
    public class Reader<T> where T : class {

        /// <summary>
        /// ���f: ���o��Ư���
        /// </summary>
        /// <param name="data_">�����</param>
        /// <returns>��Ư���</returns>
        public delegate pkey DelegatePkey(object data_);

        public Reader(DelegatePkey delegatePkey_) {
            delegatePkey = delegatePkey_;
        }

        /// <summary>
        /// �]�w���
        /// </summary>
        /// <param name="jsons_">json�r��C��</param>
        /// <returns>true��ܦ��\, false�h�_</returns>
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
        /// �]�w���
        /// </summary>
        /// <param name="json_">json�r��</param>
        /// <returns>true��ܦ��\, false�h�_</returns>
        public bool Set(string json_) {
            try {
                return Set(JsonConvert.DeserializeObject<T>(json_));
            } catch (Exception) {
                return false;
            }//try
        }

        /// <summary>
        /// �]�w���
        /// </summary>
        /// <param name="data_">��ƪ���</param>
        /// <returns>true��ܦ��\, false�h�_</returns>
        public bool Set(T data_) {
            if (data_ == null)
                return false;

            if (delegatePkey == null)
                return false;

            vaults[delegatePkey(data_)] = data_;

            return true;
        }

        /// <summary>
        /// ���o���
        /// </summary>
        /// <param name="pkey_">��Ư���</param>
        /// <returns>��ƪ���</returns>
        public T Get(pkey pkey_) {
            if (vaults.TryGetValue(pkey_, out var result))
                return result;

            return null;
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
        /// ���f: ���o��Ư���
        /// </summary>
        private DelegatePkey delegatePkey = null;

        /// <summary>
        /// ��ƦC��
        /// </summary>
        private Dictionary<pkey, T> vaults = new Dictionary<pkey, T>();
    }
}