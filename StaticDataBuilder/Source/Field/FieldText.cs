using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 字串欄位
    /// </summary>
    public class FieldText : IFieldType
    {
        public string Type() {
            return "text";
        }

        public string TypeCs() {
            return "string";
        }

        public string TypeCpp() {
            return "std::string";
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
                jsonWriter.WriteValue(value);

                return string.Empty;
            }
            catch (Exception e) {
                jsonWriter.WriteValue(string.Empty);

                return e.Message;
            }
        }
    }
}