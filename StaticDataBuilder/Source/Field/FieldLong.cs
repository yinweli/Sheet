using Newtonsoft.Json;
using System;

namespace StaticData {

    /// <summary>
    /// 長整數欄位
    /// </summary>
    public class FieldLong : IFieldType {

        public string Type() {
            return "long";
        }

        public string TypeCs() {
            return "long";
        }

        public string TypeCpp() {
            return "int64_t";
        }

        public bool IsExport() {
            return true;
        }

        public bool IsPrimaryKey() {
            return false;
        }

        public string WriteJsonObject(JsonWriter jsonWriter_, string name_, string value_) {
            jsonWriter_.WritePropertyName(name_);

            try {
                jsonWriter_.WriteValue(Convert.ToInt64(value_));

                return string.Empty;
            } catch (Exception e) {
                jsonWriter_.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}