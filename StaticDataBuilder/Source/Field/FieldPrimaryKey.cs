using Newtonsoft.Json;
using System;

namespace StaticData
{
    /// <summary>
    /// 主要索引
    /// </summary>
    public class FieldPrimaryKey : IFieldType
    {
        public string Type() {
            return "pkey";
        }

        public string TypeCs() {
            return "pkey"; // pkey型態宣告在命名空間StaticData中
        }

        public string TypeCpp() {
            return "StaticData::pkey"; // pkey型態宣告在命名空間StaticData中
        }

        public bool IsExport() {
            return true;
        }

        public bool IsPrimaryKey() {
            return true;
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