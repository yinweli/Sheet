using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 浮點數陣列欄位
    /// </summary>
    public class FieldRealArray : IFieldType
    {
        public string Type() {
            return "realArray";
        }

        public string TypeCs() {
            return "List<double>";
        }

        public string TypeCpp() {
            return "std::vector<double>";
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
                    jsonWriter.WriteValue(Convert.ToDouble(itor));
            }
            catch (Exception e) {
                result = e.Message;
            }

            jsonWriter.WriteEnd();

            return result;
        }
    }
}