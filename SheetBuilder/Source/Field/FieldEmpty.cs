using Newtonsoft.Json;

namespace Sheet {

    /// <summary>
    /// 空欄位
    /// </summary>
    public class FieldEmpty : IFieldType {

        public string Type() {
            return "empty";
        }

        public string TypeCs() {
            return string.Empty;
        }

        public string TypeCpp() {
            return string.Empty;
        }

        public bool IsExport() {
            return false;
        }

        public bool IsPrimaryKey() {
            return false;
        }

        public string WriteJsonObject(JsonWriter jsonWriter_, string name_, string value_, long pkeyStart_) {
            return string.Empty;
        }
    }
}