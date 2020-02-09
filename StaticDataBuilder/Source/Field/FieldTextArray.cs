using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 字串陣列欄位
    /// </summary>
    public class FieldTextArray : IFieldType
    {
        public string Type() {
            return "textArray";
        }

        public string TypeCs() {
            return "List<string>";
        }

        public string TypeCpp() {
            return "std::vector<std::string>";
        }

        public bool IsExport() {
            return true;
        }

        public bool IsPrimaryKey() {
            return false;
        }

        public string WriteJsonObject(JsonWriter jsonWriter, string name, string value) {
            jsonWriter.WritePropertyName(name);
            jsonWriter.WriteStartArray();

            var result = string.Empty;

            try {
                foreach (string itor in UtilityString.SplitArrayString(value))
                    jsonWriter.WriteValue(itor);
            }
            catch (Exception e) {
                result = e.Message;
            }

            jsonWriter.WriteEnd();

            return result;
        }
    }
}