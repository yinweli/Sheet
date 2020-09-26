using System;
using System.Collections.Generic;
using System.IO;

namespace Sheet {

    /// <summary>
    /// 匯出c#版本的資料結構
    /// </summary>
    public class ExportCsStruct {

        /// <summary>
        /// 程式檔案的副檔名
        /// </summary>
        public const string codeExtension = ".cs";

        /// <summary>
        /// Json檔案的副檔名
        /// </summary>
        public const string jsonExtension = ".json";

        /// <summary>
        /// 程式碼樣板
        /// </summary>
        public const string codeTemplate =
    @"// generation time={0:yyyy-MM-dd HH:mm:ss}

using System;
using System.Collections.Generic;

namespace Sheet {{
    using pkey = Int64;

    public class {1} {{
{2}
{3}
    }}
}}";

        /// <summary>
        /// 程式碼樣板: 檔案名稱
        /// </summary>
        public const string filenameTemplate = @"        public const string filename = ""{0}"";";

        /// <summary>
        /// 程式碼樣板: 欄位
        /// </summary>
        public const string fieldTemplate = @"        public {0} {1}; // {2}";

        /// <summary>
        /// 匯出到c#檔案
        /// </summary>
        /// <param name="settingGlobal_">全域設定資料</param>
        /// <param name="settingReader">讀取器設定資料</param>
        /// <param name="settingElement_">項目設定資料</param>
        /// <param name="import_">匯入器</param>
        /// <returns>true表示成功, false則否</returns>
        public static bool Export(SettingGlobal settingGlobal_, SettingElement settingElement_, Import import_) {
            var elementName = settingElement_.GetElementName();
            var filepath = Path.Combine(settingGlobal_.outputPathCs, elementName + codeExtension);
            var fields = new List<string>();

            foreach (var itor in import_.GetFields()) {
                if (itor.fieldType.IsExport())
                    fields.Add(string.Format(fieldTemplate, itor.fieldType.TypeCs(), itor.name, itor.note));
            }//for

            var context = string.Format(
                codeTemplate,
                DateTime.Now,
                elementName,
                string.Format(filenameTemplate, elementName + jsonExtension),
                string.Join(Environment.NewLine, fields));

            Directory.CreateDirectory(settingGlobal_.outputPathCs);
            UtilityFile.WriteAllText(filepath, context);

            return true;
        }
    }
}