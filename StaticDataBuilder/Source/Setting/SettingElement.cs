using System.IO;

namespace StaticData
{
    /// <summary>
    /// 項目設定資料
    /// </summary>
    public class SettingElement
    {
        /// <summary>
        /// 檢查是否正確
        /// </summary>
        /// <returns>true表示正確, false則否</returns>
        public bool Check() {
            if (excelName == string.Empty)
                return Output.Error("setting element", "excel name empty");

            if (sheetName == string.Empty)
                return Output.Error("setting element", "sheet name empty");

            if (elementName == string.Empty)
                elementName = Path.GetFileNameWithoutExtension(excelName) + "_" + sheetName;

            return true;
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
        /// 項目名稱
        /// </summary>
        public string elementName = string.Empty;
    }
}