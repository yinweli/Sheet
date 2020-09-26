using Newtonsoft.Json;

namespace Sheet {

    /// <summary>
    /// 欄位型態介面
    /// </summary>
    public interface IFieldType {

        /// <summary>
        /// 取得欄位型態字串
        /// </summary>
        /// <returns>欄位型態字串</returns>
        string Type();

        /// <summary>
        /// c#型態字串
        /// </summary>
        /// <returns>型態字串</returns>
        string TypeCs();

        /// <summary>
        /// c++型態字串
        /// </summary>
        /// <returns>型態字串</returns>
        string TypeCpp();

        /// <summary>
        /// 是否要輸出
        /// </summary>
        /// <returns>true表示要輸出, false則否</returns>
        bool IsExport();

        /// <summary>
        /// 是否是主要索引
        /// </summary>
        /// <returns>true表示是主要索引, false則否</returns>
        bool IsPrimaryKey();

        /// <summary>
        /// 寫入JsonWriter物件
        /// </summary>
        /// <param name="jsonWriter_">JsonWriter物件</param>
        /// <param name="name_">欄位名稱</param>
        /// <param name="value_">欄位字串</param>
        /// <param name="pkeyStart_">主要索引起始編號</param>
        /// <returns>如果是空字串表示成功, 否則是錯誤訊息</returns>
        string WriteJsonObject(JsonWriter jsonWriter_, string name_, string value_, long pkeyStart_);
    }

    /// <summary>
    /// 欄位
    /// </summary>
    public class Field {

        /// <summary>
        /// 從欄位字串解析欄位物件
        /// </summary>
        /// <param name="fieldString_">欄位字串</param>
        /// <returns>欄位物件</returns>
        public static Field Parse(string fieldString_) {
            string[] parts = UtilityString.SplitFieldString(fieldString_);

            if (parts.Length >= 2)
                return new Field() { meta = fieldString_, name = parts[0], fieldType = ParseField(parts[1]) };
            else
                return new Field() { meta = fieldString_ };
        }

        /// <summary>
        /// 從欄位型態字串解析欄位型態物件
        /// </summary>
        /// <param name="fieldType_">欄位型態字串</param>
        /// <returns>欄位型態物件</returns>
        private static IFieldType ParseField(string fieldType_) {
            if (fieldType_.CompareTo(empty.Type()) == 0)
                return empty;

            if (fieldType_.CompareTo(pkey.Type()) == 0)
                return pkey;

            if (fieldType_.CompareTo(@int.Type()) == 0)
                return @int;

            if (fieldType_.CompareTo(intArray.Type()) == 0)
                return intArray;

            if (fieldType_.CompareTo(@long.Type()) == 0)
                return @long;

            if (fieldType_.CompareTo(longArray.Type()) == 0)
                return longArray;

            if (fieldType_.CompareTo(real.Type()) == 0)
                return real;

            if (fieldType_.CompareTo(realArray.Type()) == 0)
                return realArray;

            if (fieldType_.CompareTo(text.Type()) == 0)
                return text;

            if (fieldType_.CompareTo(textArray.Type()) == 0)
                return textArray;

            return null;
        }

        private static IFieldType empty = new FieldEmpty();
        private static IFieldType pkey = new FieldPrimaryKey();
        private static IFieldType @int = new FieldInteger();
        private static IFieldType intArray = new FieldIntegerArray();
        private static IFieldType @long = new FieldLong();
        private static IFieldType longArray = new FieldLongArray();
        private static IFieldType real = new FieldReal();
        private static IFieldType realArray = new FieldRealArray();
        private static IFieldType text = new FieldText();
        private static IFieldType textArray = new FieldTextArray();

        /// <summary>
        /// 原始字串
        /// </summary>
        public string meta = string.Empty;

        /// <summary>
        /// 欄位名稱
        /// </summary>
        public string name = string.Empty;

        /// <summary>
        /// 註解名稱
        /// </summary>
        public string note = string.Empty;

        /// <summary>
        /// 欄位型態
        /// </summary>
        public IFieldType fieldType = null;
    }
}