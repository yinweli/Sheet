using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 整數陣列欄位
    /// </summary>
    public class FieldIntegerArray : IFieldType
    {
        public string Type() {
            return "intArray";
        }

        public string TypeCs() {
            return "List<int>";
        }

        public string TypeCpp() {
            return "std::vector<int32_t>";
        }

        public bool IsExport() {
            return true;
        }

        public bool IsPrimaryKey() {
            return false;
        }

        public string WriteJsonObject(JsonWriter jsonWriter, string name, string value) {
            jsonWriter.WritePropertyName(name);
            jsonWriter.WriteStartArray();

            var result = string.Empty;

            try {
                foreach (string itor in UtilityString.SplitArrayString(value))
                    jsonWriter.WriteValue(Convert.ToInt32(itor));
            }
            catch (Exception e) {
                result = e.Message;
            }

            jsonWriter.WriteEnd();

            return result;
        }
    }
}