using System;

namespace Sheet {

    /// <summary>
    /// 主要索引工具
    /// </summary>
    public class UtilityPkey {

        /// <summary>
        /// 表格的數字位數
        /// </summary>
        public const long sheetDigits = 100000;

        /// <summary>
        /// 主要索引的數字位數
        /// </summary>
        public const long pkeyDigits = 10000000000;

        /// <summary>
        /// 檢查主要索引起始編號是否正確
        /// </summary>
        /// <param name="pkeyStart_">主要索引起始編號</param>
        /// <returns>true表示正確, false則否</returns>
        public static bool CheckPkeyStart(long pkeyStart_) {
            return Math.Floor(Math.Log10(pkeyStart_) + 1) <= sheetDigits;
        }

        /// <summary>
        /// 檢查主要索引是否正確
        /// </summary>
        /// <param name="pkey_">主要索引</param>
        /// <returns>true表示正確, false則否</returns>
        public static bool CheckPkey(long pkey_) {
            return Math.Floor(Math.Log10(pkey_) + 1) <= pkeyDigits;
        }

        /// <summary>
        /// 正規化主要索引
        /// </summary>
        /// <param name="pkeyStart_">主要索引起始編號</param>
        /// <param name="pkey_">主要索引</param>
        /// <returns>主要索引</returns>
        public static long NormalizePkey(long pkeyStart_, long pkey_) {
            return pkeyStart_ * pkeyDigits + pkey_;
        }
    }
}