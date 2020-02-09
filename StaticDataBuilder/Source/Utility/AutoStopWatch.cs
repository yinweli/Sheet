using System;
using System.Diagnostics;

namespace StaticData
{
    /// <summary>
    /// 自動報告計時
    /// </summary>
    public class AutoStopwatch : IDisposable
    {
        /// <param name="title">標題</param>
        public AutoStopwatch(string title) {
            this.title = title;
            this.stopWatch.Start();
        }

        public void Dispose() {
            stopWatch.Stop();
            Output.Info(string.Format("{0} finish, used time={1}ms", title, stopWatch.ElapsedMilliseconds));
        }

        /// <summary>
        /// 報告標題
        /// </summary>
        private string title = string.Empty;

        /// <summary>
        /// 計時器物件
        /// </summary>
        private Stopwatch stopWatch = new Stopwatch();
    }
}