using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 浮點數欄位
    /// </summary>
    public class FieldFloat : IFieldType {

        public string Type() {
            return "float";
        }

        public string TypeCs() {
            return "float";
        }

        public string TypeCpp() {
            return "float";
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
                jsonWriter_.WriteValue((float)Convert.ToDouble(value_));

                return string.Empty;
            } catch (Exception e) {
                jsonWriter_.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}