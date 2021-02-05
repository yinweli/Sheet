using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 浮點數欄位
    /// </summary>
    public class FieldBoolean : IFieldType {

        public string Type() {
            return "bool";
        }

        public string TypeCs() {
            return "bool";
        }

        public string TypeCpp() {
            return "bool";
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
                jsonWriter_.WriteValue(Convert.ToBoolean(value_));

                return string.Empty;
            } catch (Exception e) {
                jsonWriter_.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}