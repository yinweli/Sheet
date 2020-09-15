using System;
using System.Diagnostics;
using System.IO;

namespace StaticData {

    /// <summary>
    /// 輸出訊息
    /// </summary>
    public class Output {

        /// <summary>
        /// 記錄檔名
        /// </summary>
        public const string errorLog = "error.log";

        /// <summary>
        /// 輸出一般訊息
        /// </summary>
        /// <param name="message_">訊息字串</param>
        /// <returns>只回傳true</returns>
        public static bool Info(string message_) {
            string message = string.Format(GetFormat("Info"), DateTime.Now, message_);

            Debug.WriteLine(message);
            Console.WriteLine(message);

            return true;
        }

        /// <summary>
        /// 輸出錯誤訊息
        /// </summary>
        /// <param name="message_">訊息字串</param>
        /// <returns>只回傳false</returns>
        public static bool Error(string message_) {
            string message = string.Format(GetFormat("Error"), DateTime.Now, message_);

            Debug.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine(message);
            Console.ResetColor();
            File.AppendAllText(errorLog, message + "\n");

            return false;
        }

        /// <summary>
        /// 輸出錯誤訊息
        /// </summary>
        /// <param name="title_">標題字串</param>
        /// <param name="message_">訊息字串</param>
        /// <returns>只回傳false</returns>
        public static bool Error(string title_, string message_) {
            return Error(string.Format("{0} : {1}", title_, message_));
        }

        /// <summary>
        /// 取得輸出格式
        /// </summary>
        /// <param name="type_">類型字串</param>
        /// <returns>輸出格式</returns>
        private static string GetFormat(string type_) {
            return "{0:yyyy/MM/dd HH:mm:ss} [" + type_ + "] {1}";
        }
    }
}