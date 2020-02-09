using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 長整數欄位
    /// </summary>
    public class FieldLong : IFieldType
    {
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

        public string WriteJsonObject(JsonWriter jsonWriter, string name, string value) {
            jsonWriter.WritePropertyName(name);

            try {
                jsonWriter.WriteValue(Convert.ToInt64(value));

                return string.Empty;
            }
            catch (Exception e) {
                jsonWriter.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}