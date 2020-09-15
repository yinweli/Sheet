using System.Collections.Generic;
using System.IO;

namespace StaticData {

    /// <summary>
    /// 檔案工具
    /// </summary>
    public class UtilityFile {

        /// <summary>
        /// 寫入字串列表到檔案
        /// </summary>
        /// <param name="filepath_">檔案路徑</param>
        /// <param name="contents_">字串列表</param>
        public static void WriteAllLine(string filepath_, List<string> contents_) {
            File.Delete(filepath_);
            File.WriteAllLines(filepath_, contents_);
        }

        /// <summary>
        /// 寫入字串到檔案
        /// </summary>
        /// <param name="filepath_">檔案路徑</param>
        /// <param name="content_">字串</param>
        public static void WriteAllText(string filepath_, string content_) {
            File.Delete(filepath_);
            File.WriteAllText(filepath_, content_);
        }
    }
}