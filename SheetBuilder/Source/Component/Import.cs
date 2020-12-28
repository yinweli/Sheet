using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace Sheet {

    /// <summary>
    /// Excel匯入器
    /// </summary>
    public class Import {

        /// <summary>
        /// 錯誤訊息後綴字
        /// </summary>
        public const string errorSuffix = " @import";

        /// <summary>
        /// 註解行號, 所以不需要處理
        /// </summary>
        public const int lineNote = 1;

        /// <summary>
        /// 欄位行號
        /// </summary>
        public const int lineField = 2;

        /// <summary>
        /// 資料起始行號
        /// </summary>
        public const int lineData = 3;

        /// <summary>
        /// 取得欄位列表
        /// </summary>
        /// <returns>欄位列表</returns>
        public List<Field> GetFields() {
            return fields;
        }

        /// <summary>
        /// 取得資料列表
        /// </summary>
        /// <returns>資料列表</returns>
        public List<List<string>> GetDatas() {
            return datas;
        }

        /// <summary>
        /// 匯入資料
        /// </summary>
        /// <param name="settingGlobal_">全域設定資料</param>
        /// <param name="settingElement_">項目設定資料</param>
        /// <returns>true表示成功, false則否</returns>
        public bool Read(SettingGlobal settingGlobal_, SettingElement settingElement_) {
            var filePath = Path.Combine(settingGlobal_.excelPath, settingElement_.excelName);

            if (File.Exists(filePath) == false)
                return OutputError(settingElement_.ToString(), "excel not exist");

            try {
                using (var excelPackage = new ExcelPackage(new FileInfo(filePath))) {
                    var excelWorkSheet = excelPackage.Workbook.Worksheets[settingElement_.sheetName];

                    if (excelWorkSheet == null)
                        return OutputError(settingElement_.ToString(), "sheet not exist");

                    if (excelWorkSheet.Dimension.Columns <= 0)
                        return OutputError(settingElement_.ToString(), "sheet column empty");

                    var templateFields = UtilityExcel.GetExcelRows(excelWorkSheet, lineField).Select(itor => Field.Parse(itor)).ToList();
                    var templateNotes = UtilityExcel.GetExcelRows(excelWorkSheet, lineNote);

                    if (templateFields.Count <= 0)
                        return OutputError(settingElement_.ToString(), "fields empty");

                    if (templateFields.Count != templateNotes.Count)
                        return OutputError(settingElement_.ToString(), "fields not match notes");

                    if (CheckFields(settingElement_.ToString(), templateFields) == false)
                        return OutputError(settingElement_.ToString(), "fields error");

                    if (templateFields.Where(itor => itor.fieldType.IsPrimaryKey()).Count() <= 0)
                        return OutputError(settingElement_.ToString(), "primary key not found");

                    if (templateFields.Where(itor => itor.fieldType.IsPrimaryKey()).Count() > 1)
                        return OutputError(settingElement_.ToString(), "too many primary key");

                    for (var i = 0; i < templateFields.Count; ++i)
                        templateFields[i].note = templateNotes[i];

                    fields = templateFields;
                    datas = Enumerable.Range(lineData, Math.Max(UtilityExcel.GetExcelRowCount(excelWorkSheet) - lineField, 0))
                        .Select(itor => UtilityExcel.GetExcelRows(excelWorkSheet, itor, GetFields().Count)).ToList();

                    return true;
                }//using
            } catch (Exception e) {
                return OutputError(settingElement_.ToString(), "open excel file failed, " + e.ToString());
            }
        }

        /// <summary>
        /// 取得主要索引欄位名稱
        /// </summary>
        /// <returns>主要索引欄位名稱</returns>
        public string GetPkeyName() {
            foreach (var itor in GetFields()) {
                if (itor.fieldType.IsExport() && itor.fieldType.IsPrimaryKey())
                    return itor.name;
            }//for

            return string.Empty;
        }

        /// <summary>
        /// 檢查欄位列表
        /// </summary>
        /// <param name="title_">標題字串</param>
        /// <param name="fields_">欄位列表</param>
        /// <returns>true表示成功, false則否</returns>
        private bool CheckFields(string title_, List<Field> fields_) {
            var errorFields = fields_.Where(itor => itor.fieldType == null).ToList();

            errorFields.ForEach(itor => OutputError(title_, "fields type null, field=" + itor.meta));

            return errorFields.Count <= 0;
        }

        /// <summary>
        /// 輸出錯誤訊息
        /// </summary>
        /// <param name="title_">標題字串</param>
        /// <param name="message_">訊息字串</param>
        /// <returns>只回傳false</returns>
        private bool OutputError(string title_, string message_) {
            return Output.Error(title_ + errorSuffix, message_);
        }

        /// <summary>
        /// 欄位列表
        /// </summary>
        private List<Field> fields = null;

        /// <summary>
        /// 資料列表
        /// </summary>
        private List<List<string>> datas = null;
    }
}