using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 整數陣列欄位
    /// </summary>
    public class FieldIntegerArray : IFieldType {

        public string Type() {
            return "intArray";
        }

        public string TypeCs() {
            return "List<int>";
        }

        public string TypeCpp() {
            return "std::vector<int32_t>";
        }

        public bool IsExport() {
            return true;
        }

        public bool IsPrimaryKey() {
            return false;
        }

        public string WriteJsonObject(JsonWriter jsonWriter_, string name_, string value_, long pkeyStart_) {
            jsonWriter_.WritePropertyName(name_);
            jsonWriter_.WriteStartArray();

            var result = string.Empty;

            try {
                foreach (string itor in UtilityString.SplitArrayString(value_))
                    jsonWriter_.WriteValue(Convert.ToInt32(itor));
            } catch (Exception e) {
                result = e.Message;
            }

            jsonWriter_.WriteEnd();

            return result;
        }
    }
}