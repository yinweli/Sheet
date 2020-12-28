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

        public string WriteJsonObject(JsonWriter jsonWriter_, string name_, string value_, long pkeyStart_) {
            jsonWriter_.WritePropertyName(name_);

            try {
                var pkey = Convert.ToInt64(value_);

                if (UtilityPkey.CheckPkey(pkey) == false)
                    throw new Exception("pkey too large");

                jsonWriter_.WriteValue(UtilityPkey.NormalizePkey(pkeyStart_, pkey));

                return string.Empty;
            } catch (Exception e) {
                jsonWriter_.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}