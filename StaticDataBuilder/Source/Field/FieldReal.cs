using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 浮點數欄位
    /// </summary>
    public class FieldReal : IFieldType
    {
        public string Type() {
            return "real";
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

        public string WriteJsonObject(JsonWriter jsonWriter, string name, string value) {
            jsonWriter.WritePropertyName(name);

            try {
                jsonWriter.WriteValue(Convert.ToDouble(value));

                return string.Empty;
            }
            catch (Exception e) {
                jsonWriter.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}