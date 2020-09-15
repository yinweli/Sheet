namespace StaticData {

    /// <summary>
    /// 字串工具
    /// </summary>
    public class UtilityString {

        /// <summary>
        /// 欄位分隔符號
        /// </summary>
        public const string fieldSeparator = "#";

        /// <summary>
        /// 陣列分隔符號
        /// </summary>
        public const string arraySeparator = ";";

        /// <summary>
        /// 取得分割後的欄位字串陣列
        /// </summary>
        /// <param name="fieldString_">欄位字串</param>
        /// <returns>欄位字串陣列</returns>
        public static string[] SplitFieldString(string fieldString_) {
            return fieldString_.Split(fieldSeparator.ToCharArray());
        }

        /// <summary>
        /// 取得分割後的陣列字串陣列
        /// </summary>
        /// <param name="arrayString_">陣列字串</param>
        /// <returns>陣列字串陣列</returns>
        public static string[] SplitArrayString(string arrayString_) {
            return arrayString_.Split(arraySeparator.ToCharArray());
        }
    }
}