using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 整數欄位
    /// </summary>
    public class FieldInteger : IFieldType {

        public string Type() {
            return "int";
        }

        public string TypeCs() {
            return "int";
        }

        public string TypeCpp() {
            return "int32_t";
        }

        public bool IsExport() {
            return true;
        }

        public bool IsPrimaryKey() {
            return false;
        }

        public string WriteJsonObject(JsonWriter jsonWriter_, string name_, string value_, long pkeyStart_) {
            jsonWriter_.WritePropertyName(name_);

            try {
                jsonWriter_.WriteValue(Convert.ToInt32(value_));

                return string.Empty;
            } catch (Exception e) {
                jsonWriter_.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}