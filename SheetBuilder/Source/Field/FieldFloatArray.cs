using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 浮點數陣列欄位
    /// </summary>
    public class FieldFloatArray : IFieldType {

        public string Type() {
            return "floatArray";
        }

        public string TypeCs() {
            return "List<float>";
        }

        public string TypeCpp() {
            return "std::vector<float>";
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
                    jsonWriter_.WriteValue((float)Convert.ToDouble(itor));
            } catch (Exception e) {
                result = e.Message;
            }

            jsonWriter_.WriteEnd();

            return result;
        }
    }
}