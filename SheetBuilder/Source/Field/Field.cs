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
            if (fieldType_.CompareTo(fieldEmpty.Type()) == 0)
                return fieldEmpty;

            if (fieldType_.CompareTo(fieldPkey.Type()) == 0)
                return fieldPkey;

            if (fieldType_.CompareTo(fieldBool.Type()) == 0)
                return fieldBool;

            if (fieldType_.CompareTo(fieldBoolArray.Type()) == 0)
                return fieldBoolArray;

            if (fieldType_.CompareTo(fieldInt.Type()) == 0)
                return fieldInt;

            if (fieldType_.CompareTo(fieldIntArray.Type()) == 0)
                return fieldIntArray;

            if (fieldType_.CompareTo(fieldLong.Type()) == 0)
                return fieldLong;

            if (fieldType_.CompareTo(fieldLongArray.Type()) == 0)
                return fieldLongArray;

            if (fieldType_.CompareTo(fieldFloat.Type()) == 0)
                return fieldFloat;

            if (fieldType_.CompareTo(fieldFloatArray.Type()) == 0)
                return fieldFloatArray;

            if (fieldType_.CompareTo(fieldDouble.Type()) == 0)
                return fieldDouble;

            if (fieldType_.CompareTo(fieldDoubleArray.Type()) == 0)
                return fieldDoubleArray;

            if (fieldType_.CompareTo(fieldText.Type()) == 0)
                return fieldText;

            if (fieldType_.CompareTo(fieldTextArray.Type()) == 0)
                return fieldTextArray;

            return null;
        }

        private static IFieldType fieldEmpty = new FieldEmpty();
        private static IFieldType fieldPkey = new FieldPrimaryKey();
        private static IFieldType fieldBool = new FieldBoolean();
        private static IFieldType fieldBoolArray = new FieldBooleanArray();
        private static IFieldType fieldInt = new FieldInteger();
        private static IFieldType fieldIntArray = new FieldIntegerArray();
        private static IFieldType fieldLong = new FieldLong();
        private static IFieldType fieldLongArray = new FieldLongArray();
        private static IFieldType fieldFloat = new FieldFloat();
        private static IFieldType fieldFloatArray = new FieldFloatArray();
        private static IFieldType fieldDouble = new FieldDouble();
        private static IFieldType fieldDoubleArray = new FieldDoubleArray();
        private static IFieldType fieldText = new FieldText();
        private static IFieldType fieldTextArray = new FieldTextArray();

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