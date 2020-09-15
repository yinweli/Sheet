using Newtonsoft.Json;
using System;

namespace StaticData {

    /// <summary>
    /// 長整數陣列欄位
    /// </summary>
    public class FieldLongArray : IFieldType {

        public string Type() {
            return "longArray";
        }

        public string TypeCs() {
            return "List<long>";
        }

        public string TypeCpp() {
            return "std::vector<int64_t>";
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
                    jsonWriter_.WriteValue(Convert.ToInt64(itor));
            } catch (Exception e) {
                result = e.Message;
            }

            jsonWriter_.WriteEnd();

            return result;
        }
    }
}