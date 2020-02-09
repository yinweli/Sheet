namespace StaticData
{
    /// <summary>
    /// 字串工具
    /// </summary>
    public class UtilityString
    {
        /// <summary>
        /// 取得分割後的欄位字串陣列
        /// </summary>
        /// <param name="fieldString">欄位字串</param>
        /// <returns>欄位字串陣列</returns>
        public static string[] SplitFieldString(string fieldString) {
            return fieldString.Split(TOKEN_FIELD_SEPARATOR.ToCharArray());
        }

        /// <summary>
        /// 取得分割後的陣列字串陣列
        /// </summary>
        /// <param name="arrayString">陣列字串</param>
        /// <returns>陣列字串陣列</returns>
        public static string[] SplitArrayString(string arrayString) {
            return arrayString.Split(TOKEN_ARRAY_SEPARATOR.ToCharArray());
        }

        /// <summary>
        /// 欄位分隔符號
        /// </summary>
        private const string TOKEN_FIELD_SEPARATOR = "#";

        /// <summary>
        /// 陣列分隔符號
        /// </summary>
        private const string TOKEN_ARRAY_SEPARATOR = ";";
    }
}