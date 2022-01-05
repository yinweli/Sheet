using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 布林值陣列欄位
    /// </summary>
    public class FieldBooleanArray : IFieldType {

        public string Type() {
            return "boolArray";
        }

        public string TypeCs() {
            return "List<bool>";
        }

        public string TypeCpp() {
            return "std::vector<bool>";
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
                    jsonWriter_.WriteValue(UtilityString.StringToBoolean(itor));
            } catch (Exception e) {
                result = e.Message;
            }

            jsonWriter_.WriteEnd();

            return result;
        }
    }
}