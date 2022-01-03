using OfficeOpenXml;
using System.Collections.Generic;

namespace Sheet {

    /// <summary>
    /// Excel工具
    /// </summary>
    public class UtilityExcel {

        /// <summary>
        /// 取得Execel表單中行數
        /// </summary>
        /// <param name="excelWorkSheet_">Excel表單</param>
        /// <param name="row_">起始行號, 從1起算</param>
        /// <returns>行數</returns>
        public static int GetExcelRowCount(ExcelWorksheet excelWorkSheet_, int row_) {
            var count = 0;

            for (int i = row_; i <= excelWorkSheet_.Dimension.End.Row; ++i, ++count) {
                var cell = excelWorkSheet_.Cells[i, 1];

                if (cell == null || cell.Value == null || cell.Value.ToString().Length <= 0)
                    break;
            }//for

            return count;
        }

        /// <summary>
        /// 從Excel表單中以字串列表的形式取得指定行的內容
        /// 遇到空白格時會停止
        /// </summary>
        /// <param name="excelWorkSheet_">Excel表單</param>
        /// <param name="row_">行號, 從1起算</param>
        /// <returns>字串列表</returns>
        public static List<string> GetExcelRows(ExcelWorksheet excelWorkSheet_, int row_) {
            List<string> rows = new List<string>();

            for (int i = 1; i <= excelWorkSheet_.Dimension.End.Column; ++i) {
                var cell = excelWorkSheet_.Cells[row_, i];

                if (cell == null || cell.Value == null || cell.Value.ToString().Length <= 0)
                    return rows;

                rows.Add(cell != null && cell.Value != null ? cell.Value.ToString() : string.Empty);
            }//for

            return rows;
        }

        /// <summary>
        /// 從Excel表單中以字串列表的形式取得指定行的內容
        /// 讀取到指定的欄號
        /// </summary>
        /// <param name="excelWorkSheet_">Excel表單</param>
        /// <param name="row_">行號, 從1起算</param>
        /// <param name="column_">欄號, 從1起算</param>
        /// <returns>字串列表</returns>
        public static List<string> GetExcelRows(ExcelWorksheet excelWorkSheet_, int row_, int column_) {
            List<string> rows = new List<string>();

            for (int i = 1; i <= column_; ++i) {
                var cell = excelWorkSheet_.Cells[row_, i];

                rows.Add(cell != null && cell.Value != null ? cell.Value.ToString() : string.Empty);
            }//for

            return rows;
        }
    }
}