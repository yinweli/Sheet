using System;
using System.Collections.Generic;
using System.IO;

namespace StaticData
{
    /// <summary>
    /// 匯出c#版本的資料結構
    /// </summary>
    public class ExportCsStruct
    {
        /// <summary>
        /// 匯出到c#檔案
        /// </summary>
        /// <param name="settingGlobal">全域設定資料</param>
        /// <param name="settingReader">讀取器設定資料</param>
        /// <param name="settingElement">項目設定資料</param>
        /// <param name="import">匯入器</param>
        /// <returns>true表示成功, false則否</returns>
        public static bool Export(SettingGlobal settingGlobal, SettingElement settingElement, Import import) {
            var filepath = Path.Combine(settingGlobal.outputPathCs, settingElement.elementName + EXPORT_EXT);
            var templateField = new List<string>();

            foreach (var itor in import.GetFields()) {
                if (itor.fieldType.IsExport())
                    templateField.Add(string.Format(TEMPLATE_FIELD, itor.fieldType.TypeCs(), itor.name, itor.note));
            }//for

            var context = string.Format(TEMPLATE_CODE, DateTime.Now, settingElement.elementName,
                string.Join("\n", templateField),
                string.Format(TEMPLATE_FILENAME, settingElement.elementName + EXPORT_JSON));

            Directory.CreateDirectory(settingGlobal.outputPathCs);
            UtilityFile.WriteAllText(filepath, context);

            return true;
        }

        /// <summary>
        /// 匯出副檔名
        /// </summary>
        private const string EXPORT_EXT = ".cs";

        /// <summary>
        /// 匯出Json檔名
        /// </summary>
        private const string EXPORT_JSON = ".json";

        /// <summary>
        /// 程式碼樣板
        /// </summary>
        private const string TEMPLATE_CODE =
    @"// generation time={0:yyyy-MM-dd HH:mm:ss}

using System;
using System.Collections.Generic;

namespace StaticData
{{
    using pkey = Int64;

    public class {1}
    {{
{2}
{3}
    }}
}}";

        /// <summary>
        /// 程式碼樣板: 欄位
        /// </summary>
        private const string TEMPLATE_FIELD = @"        public {0} {1}; // {2}";

        /// <summary>
        /// 程式碼樣板: 檔案名稱
        /// </summary>
        private const string TEMPLATE_FILENAME = @"        public const string filename = ""{0}"";";
    }
}