using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Sheet {

    /// <summary>
    /// Extension methods for Exception class.
    /// </summary>
    internal static class ExceptionExtensions {

        /// <summary>
        ///  Provides full stack trace for the exception that occurred.
        /// </summary>
        /// <param name="exception_">Exception object.</param>
        /// <param name="environmentStackTrace_">Environment stack trace, for pulling additional stack frames.</param>
        public static string ToLogString(this Exception exception_, string environmentStackTrace_) {
            List<string> environmentStackTraceLines = GetUserStackTraceLines(environmentStackTrace_);

            environmentStackTraceLines.RemoveAt(0);

            List<string> stackTraceLines = GetStackTraceLines(exception_.StackTrace);

            stackTraceLines.AddRange(environmentStackTraceLines);

            return exception_.Message + Environment.NewLine + String.Join(Environment.NewLine, stackTraceLines.ToArray());
        }

        /// <summary>
        ///  Gets a list of stack frame lines, as strings.
        /// </summary>
        /// <param name="stackTrace_">Stack trace string.</param>
        private static List<string> GetStackTraceLines(string stackTrace_) {
            return stackTrace_.Split(new[] { Environment.NewLine }, StringSplitOptions.None).ToList();
        }

        /// <summary>
        ///  Gets a list of stack frame lines, as strings, only including those for which line number is known.
        /// </summary>
        /// <param name="fullStackTrace_">Full stack trace, including external code.</param>
        private static List<string> GetUserStackTraceLines(string fullStackTrace_) {
            List<string> outputList = new List<string>();
            Regex regex = new Regex(@"([^\)]*\)) in (.*):line (\d)*$");

            foreach (string stackTraceLine in GetStackTraceLines(fullStackTrace_)) {
                if (regex.IsMatch(stackTraceLine))
                    outputList.Add(stackTraceLine);
            }//for

            return outputList;
        }
    }
}