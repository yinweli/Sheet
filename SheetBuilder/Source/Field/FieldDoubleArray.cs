using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 浮點數陣列欄位
    /// </summary>
    public class FieldDoubleArray : IFieldType {

        public string Type() {
            return "doubleArray";
        }

        public string TypeCs() {
            return "List<double>";
        }

        public string TypeCpp() {
            return "std::vector<double>";
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
                    jsonWriter_.WriteValue(Convert.ToDouble(itor));
            } catch (Exception e) {
                result = e.Message;
            }

            jsonWriter_.WriteEnd();

            return result;
        }
    }
}