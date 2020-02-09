using System;
using System.Diagnostics;
using System.IO;

namespace StaticData
{
    /// <summary>
    /// 輸出訊息
    /// </summary>
    public class Output
    {
        /// <summary>
        /// 輸出一般訊息
        /// </summary>
        /// <param name="message">訊息字串</param>
        /// <returns>只回傳true</returns>
        public static bool Info(string message) {
            string temp = string.Format(GetFormat("Info"), DateTime.Now, message);

            Debug.WriteLine(temp);
            Console.WriteLine(temp);

            return true;
        }

        /// <summary>
        /// 輸出錯誤訊息
        /// </summary>
        /// <param name="message">訊息字串</param>
        /// <returns>只回傳false</returns>
        public static bool Error(string message) {
            string temp = string.Format(GetFormat("Error"), DateTime.Now, message);

            Debug.WriteLine(temp);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(temp);
            Console.ResetColor();
            File.AppendAllText(errorLog, temp + "\n");

            return false;
        }

        /// <summary>
        /// 輸出錯誤訊息
        /// </summary>
        /// <param name="title">標題字串</param>
        /// <param name="message">訊息字串</param>
        /// <returns>只回傳false</returns>
        public static bool Error(string title, string message) {
            return Error(string.Format("{0} : {1}", title, message));
        }

        /// <summary>
        /// 取得輸出格式
        /// </summary>
        /// <param name="type">類型字串</param>
        /// <returns>輸出格式</returns>
        private static string GetFormat(string type) {
            return "{0:yyyy/MM/dd HH:mm:ss} [" + type + "] {1}";
        }

        /// <summary>
        /// 記錄檔名
        /// </summary>
        public const string errorLog = "error.log";
    }
}