namespace StaticData
{
    /// <summary>
    /// 全域設定資料
    /// </summary>
    public class SettingGlobal
    {
        /// <summary>
        /// 檢查是否正確
        /// </summary>
        /// <returns>true表示正確, false則否</returns>
        public bool Check() {
            if (excelPath == null)
                return Output.Error("setting global", "excelPath not found");

            if (excelPath == string.Empty)
                return Output.Error("setting global", "excelPath empty");

            if (outputPathJson == null)
                return Output.Error("setting global", "outputPathJson not found");

            if (outputPathJson == string.Empty)
                return Output.Error("setting global", "outputPathJson empty");

            if (outputPathCs == null)
                return Output.Error("setting global", "outputPathCs not found");

            if (outputPathCs == string.Empty)
                return Output.Error("setting global", "outputPathCs empty");

            if (outputPathCpp == null)
                return Output.Error("setting global", "outputPathCpp not found");

            if (outputPathCpp == string.Empty)
                return Output.Error("setting global", "outputPathCpp empty");

            if (cppLibraryPath == null)
                cppLibraryPath = string.Empty;

            return true;
        }

        /// <summary>
        /// Excel路徑
        /// </summary>
        public string excelPath = null;

        /// <summary>
        /// 輸出路徑: json
        /// </summary>
        public string outputPathJson = null;

        /// <summary>
        /// 輸出路徑: c#
        /// </summary>
        public string outputPathCs = null;

        /// <summary>
        /// 輸出路徑: cpp
        /// </summary>
        public string outputPathCpp = null;

        /// <summary>
        /// 函式庫路徑: cpp
        /// </summary>
        public string cppLibraryPath = null;
    }
}