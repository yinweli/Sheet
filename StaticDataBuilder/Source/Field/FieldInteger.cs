using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 整數欄位
    /// </summary>
    public class FieldInteger : IFieldType
    {
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

        public string WriteJsonObject(JsonWriter jsonWriter, string name, string value) {
            jsonWriter.WritePropertyName(name);

            try {
                jsonWriter.WriteValue(Convert.ToInt32(value));

                return string.Empty;
            }
            catch (Exception e) {
                jsonWriter.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}