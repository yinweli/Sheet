using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace StaticData
{
    /// <summary>
    /// 匯出到json檔案
    /// </summary>
    public class ExportJson
    {
        /// <summary>
        /// 匯出到json檔案
        /// </summary>
        /// <param name="settingGlobal">全域設定資料</param>
        /// <param name="settingElement">項目設定資料</param>
        /// <param name="import">匯入器</param>
        /// <returns>true表示成功, false則否</returns>
        public static bool Export(SettingGlobal settingGlobal, SettingElement settingElement, Import import) {
            var filepath = Path.Combine(settingGlobal.outputPathJson, settingElement.elementName + EXPORT_EXT);
            var fileContent = new List<string>();
            var fields = import.GetFields();
            var datas = import.GetDatas();
            var result = true;

            for (int row = 0; row < datas.Count; ++row) {
                var data = datas[row];
                var stringBuilder = new StringBuilder();
                var stringWriter = new StringWriter(stringBuilder);

                using (var jsonWriter = new JsonTextWriter(stringWriter)) {
                    jsonWriter.WriteStartObject();

                    for (int column = 0; column < data.Count; ++column) {
                        var field = fields[column];

                        if (field.fieldType.IsExport()) {
                            var writeResult = field.fieldType.WriteJsonObject(jsonWriter, field.name, data[column]);

                            if (writeResult.CompareTo(string.Empty) != 0)
                                result &= OutputError(settingElement.ToString(), "data error, field=" + field.name + ", row=" + row + ", error=" + writeResult);
                        }//if
                    }//for

                    jsonWriter.WriteEndObject();
                }//using

                fileContent.Add(stringBuilder.ToString());
            }//for

            Directory.CreateDirectory(settingGlobal.outputPathJson);
            UtilityFile.WriteAllLine(filepath, fileContent);

            return result;
        }

        /// <summary>
        /// 輸出錯誤訊息
        /// </summary>
        /// <param name="title">標題字串</param>
        /// <param name="message">訊息字串</param>
        /// <returns>只回傳false</returns>
        private static bool OutputError(string title, string message) {
            return Output.Error(title + ERROR_SUFFIX, message);
        }

        /// <summary>
        /// 錯誤訊息後綴字
        /// </summary>
        private const string ERROR_SUFFIX = " @exportJson";

        /// <summary>
        /// 匯出副檔名
        /// </summary>
        private const string EXPORT_EXT = ".json";
    }
}