using Newtonsoft.Json;
using System;

namespace Sheet {

    /// <summary>
    /// 主要索引
    /// </summary>
    public class FieldPrimaryKey : IFieldType {

        public string Type() {
            return "pkey";
        }

        public string TypeCs() {
            return "int";
        }

        public string TypeCpp() {
            return "Sheet::pkey"; // pkey型態宣告在命名空間Sheet中
        }

        public bool IsExport() {
            return true;
        }

        public bool IsPrimaryKey() {
            return true;
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