using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;

namespace StaticData
{
    /// <summary>
    /// Excel匯入器
    /// </summary>
    public class Import
    {
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
        /// <param name="settingGlobal">全域設定資料</param>
        /// <param name="settingElement">項目設定資料</param>
        /// <returns>true表示成功, false則否</returns>
        public bool Read(SettingGlobal settingGlobal, SettingElement settingElement) {
            var filePath = Path.Combine(settingGlobal.excelPath, settingElement.excelName);

            if (File.Exists(filePath) == false)
                return OutputError(settingElement.ToString(), "excel not exist");

            try {
                using (var excelPackage = new ExcelPackage(new FileInfo(filePath))) {
                    var excelWorkSheet = excelPackage.Workbook.Worksheets[settingElement.sheetName];

                    if (excelWorkSheet == null)
                        return OutputError(settingElement.ToString(), "sheet not exist");

                    if (excelWorkSheet.Dimension.Columns <= 0)
                        return OutputError(settingElement.ToString(), "sheet column empty");

                    var templateFields = UtilityExcel.GetExcelRows(excelWorkSheet, LINE_FIELD).Select(itor => Field.Parse(itor)).ToList();
                    var templateNotes = UtilityExcel.GetExcelRows(excelWorkSheet, LINE_NOTE);

                    if (templateFields.Count <= 0)
                        return OutputError(settingElement.ToString(), "fields empty");

                    if (templateFields.Count != templateNotes.Count)
                        return OutputError(settingElement.ToString(), "fields not match notes");

                    if (CheckFields(settingElement.ToString(), templateFields) == false)
                        return OutputError(settingElement.ToString(), "fields error");

                    if (templateFields.Where(itor => itor.fieldType.IsPrimaryKey()).Count() <= 0)
                        return OutputError(settingElement.ToString(), "primary key not found");

                    if (templateFields.Where(itor => itor.fieldType.IsPrimaryKey()).Count() > 1)
                        return OutputError(settingElement.ToString(), "too many primary key");

                    for (var i = 0; i < templateFields.Count; ++i)
                        templateFields[i].note = templateNotes[i];

                    fields = templateFields;
                    datas = Enumerable.Range(LINE_DATA, Math.Max(UtilityExcel.GetExcelRowCount(excelWorkSheet) - LINE_FIELD, 0))
                        .Select(itor => UtilityExcel.GetExcelRows(excelWorkSheet, itor, GetFields().Count)).ToList();

                    return true;
                }//using
            }
            catch (Exception e) {
                return OutputError(settingElement.ToString(), "open excel file failed, " + e.ToString());
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
        /// <param name="title">標題字串</param>
        /// <param name="fields">欄位列表</param>
        /// <returns>true表示成功, false則否</returns>
        private bool CheckFields(string title, List<Field> fields) {
            var errorFields = fields.Where(itor => itor.fieldType == null).ToList();

            errorFields.ForEach(itor => OutputError(title, "fields type null, field=" + itor.meta));

            return errorFields.Count <= 0;
        }

        /// <summary>
        /// 輸出錯誤訊息
        /// </summary>
        /// <param name="title">標題字串</param>
        /// <param name="message">訊息字串</param>
        /// <returns>只回傳false</returns>
        private bool OutputError(string title, string message) {
            return Output.Error(title + ERROR_SUFFIX, message);
        }

        /// <summary>
        /// 錯誤訊息後綴字
        /// </summary>
        private const string ERROR_SUFFIX = " @import";

        /// <summary>
        /// 註解行號, 所以不需要處理
        /// </summary>
        private const int LINE_NOTE = 1;

        /// <summary>
        /// 欄位行號
        /// </summary>
        private const int LINE_FIELD = 2;

        /// <summary>
        /// 資料起始行號
        /// </summary>
        private const int LINE_DATA = 3;

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