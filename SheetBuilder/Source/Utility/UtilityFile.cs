using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Sheet {

    /// <summary>
    /// 檔案工具
    /// </summary>
    public class UtilityFile {

        /// <summary>
        /// 寫入字串列表到檔案
        /// </summary>
        /// <param name="filepath_">檔案路徑</param>
        /// <param name="contents_">字串列表</param>
        /// <param name="bom_">是否要使用順序標記(BOM)</param>
        public static void WriteAllLine(string filepath_, List<string> contents_, bool bom_) {
            File.Delete(filepath_);
            File.WriteAllLines(filepath_, contents_, new UTF8Encoding(bom_));
        }

        /// <summary>
        /// 寫入字串到檔案
        /// </summary>
        /// <param name="filepath_">檔案路徑</param>
        /// <param name="content_">字串</param>
        /// <param name="bom_">是否要使用順序標記(BOM)</param>
        public static void WriteAllText(string filepath_, string content_, bool bom_) {
            File.Delete(filepath_);
            File.WriteAllText(filepath_, content_, new UTF8Encoding(bom_));
        }
    }
}