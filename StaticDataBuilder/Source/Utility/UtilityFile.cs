using System.Collections.Generic;
using System.IO;

namespace StaticData
{
    /// <summary>
    /// 檔案工具
    /// </summary>
    public class UtilityFile
    {
        /// <summary>
        /// 寫入字串列表到檔案
        /// </summary>
        /// <param name="filepath">檔案路徑</param>
        /// <param name="contents">字串列表</param>
        public static void WriteAllLine(string filepath, List<string> contents) {
            File.Delete(filepath);
            File.WriteAllLines(filepath, contents);
        }

        /// <summary>
        /// 寫入字串到檔案
        /// </summary>
        /// <param name="filepath">檔案路徑</param>
        /// <param name="content">字串</param>
        public static void WriteAllText(string filepath, string content) {
            File.Delete(filepath);
            File.WriteAllText(filepath, content);
        }
    }
}