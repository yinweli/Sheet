using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sheet {

    /// <summary>
    /// 匯出到json檔案
    /// </summary>
    public class ExportJson {

        /// <summary>
        /// Json檔案的副檔名
        /// </summary>
        public const string jsonExtension = ".json";

        /// <summary>
        /// 錯誤訊息後綴字
        /// </summary>
        public const string errorSuffix = " @exportJson";

        /// <summary>
        /// 匯出到json檔案
        /// </summary>
        /// <param name="settingGlobal_">全域設定資料</param>
        /// <param name="settingElement_">項目設定資料</param>
        /// <param name="import_">匯入器</param>
        /// <returns>true表示成功, false則否</returns>
        public static bool Export(SettingGlobal settingGlobal_, SettingElement settingElement_, Import import_) {
            var elementName = settingElement_.GetElementName();
            var filepath = Path.Combine(settingGlobal_.outputPathJson, elementName + jsonExtension);
            var fileContent = new List<string>();
            var pkeys = new HashSet<string>();
            var fields = import_.GetFields();
            var datas = import_.GetDatas();
            var result = true;

            for (int row = 0; row < datas.Count; ++row) {
                var data = datas[row];
                var stringBuilder = new StringBuilder();
                var stringWriter = new StringWriter(stringBuilder);

                using (var jsonWriter = new JsonTextWriter(stringWriter)) {
                    jsonWriter.WriteStartObject();

                    for (int column = 0; column < data.Count; ++column) {
                        var field = fields[column];
                        var value = data[column];

                        if (field.fieldType.IsExport()) {
                            var writeResult = field.fieldType.WriteJsonObject(jsonWriter, field.name, value, settingElement_.pkeyStart);

                            if (writeResult.Length > 0)
                                result &= OutputError(settingElement_.ToString(), "data error, field=" + field.name + ", row=" + row + ", error=" + writeResult);
                        }//if

                        if (field.fieldType.IsPrimaryKey()) {
                            if (pkeys.Contains(value))
                                result &= OutputError(settingElement_.ToString(), "data error, field=" + field.name + ", row=" + row + ", error=pkey duplicate");
                            else
                                pkeys.Add(value);
                        }//if
                    }//for

                    jsonWriter.WriteEndObject();
                }//using

                fileContent.Add(stringBuilder.ToString());
            }//for

            Directory.CreateDirectory(settingGlobal_.outputPathJson);
            UtilityFile.WriteAllLine(filepath, fileContent, settingGlobal_.bom);

            return result;
        }

        /// <summary>
        /// 輸出錯誤訊息
        /// </summary>
        /// <param name="title_">標題字串</param>
        /// <param name="message_">訊息字串</param>
        /// <returns>只回傳false</returns>
        private static bool OutputError(string title_, string message_) {
            return Output.Error(title_ + errorSuffix, message_);
        }
    }
}