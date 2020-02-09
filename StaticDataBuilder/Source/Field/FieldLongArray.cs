using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 長整數陣列欄位
    /// </summary>
    public class FieldLongArray : IFieldType
    {
        public string Type() {
            return "longArray";
        }

        public string TypeCs() {
            return "List<long>";
        }

        public string TypeCpp() {
            return "std::vector<int64_t>";
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
                    jsonWriter.WriteValue(Convert.ToInt64(itor));
            }
            catch (Exception e) {
                result = e.Message;
            }

            jsonWriter.WriteEnd();

            return result;
        }
    }
}