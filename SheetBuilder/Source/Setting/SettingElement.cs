using System.IO;

namespace Sheet {

    /// <summary>
    /// 項目設定資料
    /// </summary>
    public class SettingElement {

        /// <summary>
        /// 檢查是否正確
        /// </summary>
        /// <returns>true表示正確, false則否</returns>
        public bool Check() {
            if (excelName == string.Empty)
                return Output.Error("setting element", "excel name empty");

            if (sheetName == string.Empty)
                return Output.Error("setting element", "sheet name empty");

            if (pkeyStart <= 0)
                return Output.Error("setting element", "pkey start too small");

            if (UtilityPkey.CheckPkeyStart(pkeyStart) == false)
                return Output.Error("setting element", "pkey start too large");

            return true;
        }

        /// <summary>
        /// 取得項目名稱
        /// </summary>
        /// <returns>項目名稱</returns>
        public string GetElementName() {
            return Path.GetFileNameWithoutExtension(excelName) + sheetName;
        }

        public override string ToString() {
            return string.Format("{0} # {1}", excelName, sheetName);
        }

        /// <summary>
        /// Excel檔名
        /// </summary>
        public string excelName = string.Empty;

        /// <summary>
        /// Excel表單名稱
        /// </summary>
        public string sheetName = string.Empty;

        /// <summary>
        /// 主要索引起始編號
        /// </summary>
        public long pkeyStart = 0;
    }
}