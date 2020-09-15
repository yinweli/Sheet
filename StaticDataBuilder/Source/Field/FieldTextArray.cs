using Newtonsoft.Json;
using System;

namespace StaticData {

    /// <summary>
    /// 字串陣列欄位
    /// </summary>
    public class FieldTextArray : IFieldType {

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

        public string WriteJsonObject(JsonWriter jsonWriter_, string name_, string value_) {
            jsonWriter_.WritePropertyName(name_);
            jsonWriter_.WriteStartArray();

            var result = string.Empty;

            try {
                foreach (string itor in UtilityString.SplitArrayString(value_))
                    jsonWriter_.WriteValue(itor);
            } catch (Exception e) {
                result = e.Message;
            }

            jsonWriter_.WriteEnd();

            return result;
        }
    }
}