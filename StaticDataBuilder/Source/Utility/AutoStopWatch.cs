using System;
using System.Diagnostics;

namespace StaticData {

    /// <summary>
    /// 自動報告計時
    /// </summary>
    public class AutoStopwatch : IDisposable {

        public AutoStopwatch(string title_) {
            title = title_;
            stopwatch.Start();
        }

        public void Dispose() {
            stopwatch.Stop();
            Output.Info(string.Format("{0} finish, used time={1}ms", title, stopwatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// 報告標題
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// 計時器物件
        /// </summary>
        private Stopwatch stopwatch = new Stopwatch();
    }
}