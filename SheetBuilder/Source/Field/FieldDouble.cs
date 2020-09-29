using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 浮點數欄位
    /// </summary>
    public class FieldDouble : IFieldType {

        public string Type() {
            return "double";
        }

        public string TypeCs() {
            return "double";
        }

        public string TypeCpp() {
            return "double";
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
                jsonWriter_.WriteValue(Convert.ToDouble(value_));

                return string.Empty;
            } catch (Exception e) {
                jsonWriter_.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}