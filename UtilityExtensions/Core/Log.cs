using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.CompilerServices;

namespace UtilityExtensions.Core
{
    public static class Log
    {
        public interface IExceptionMessageHandler
        {
            string GetMessage(Exception exception);

            Type GetExceptionType();
        }

        public class ExceptionMessageHandler<T> : IExceptionMessageHandler where T : Exception
        {
            private readonly Func<T, string> getMessage;

            public ExceptionMessageHandler(Func<T, string> getMessage)
            {
                this.getMessage = getMessage;
            }

            public string GetMessage(Exception exception)
            {
                return getMessage((T)exception);
            }

            public Type GetExceptionType()
            {
                return typeof(T);
            }
        }

        [Flags]
        public enum ExceptionFlags
        {
            /// <summary>
            /// No extra information, shows only the exception message
            /// </summary>
            None = 0,

            /// <summary>
            /// Show the name of the file where the exception happened
            /// </summary>
            FileName = 1,

            /// <summary>
            /// Show the exception's type
            /// </summary>
            ExceptionType = 2,

            /// <summary>
            /// Show the name of the method where the exception happened
            /// </summary>
            MethodName = 4,

            /// <summary>
            /// Show the line number where the exception happened
            /// </summary>
            Line = 8,

            /// <summary>
            /// Show all information
            /// </summary>
            All = FileName | ExceptionType | MethodName | Line
        }

        /// <summary>
        /// String representation of the current time in 'yyyy-MM-dd HH:mm:ss' format
        /// </summary>
        public static string Time => DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        public static string Exception(Exception exception,
                                       ExceptionFlags flags = ExceptionFlags.All,
                                       List<IExceptionMessageHandler> exceptionMessageHandlers = null,
                                       [CallerFilePath] string path = "")
        {
            if (exception == null)
            {
                return "";
            }

            StackTrace st = new(exception, true);

            //Get the first stack frame
            StackFrame frame = st.GetFrame(st.FrameCount - 1);

            List<string> namesList = new();

            if (flags.HasFlag(ExceptionFlags.FileName))
            {
                //Get the file name
                string fileName = Path.GetFileName(path);

                if (!string.IsNullOrEmpty(fileName))
                {
                    namesList.Add(fileName);
                }
            }

            if (flags.HasFlag(ExceptionFlags.MethodName))
            {
                //Get the method
                System.Reflection.MethodBase methodInfo = frame.GetMethod();

                string methodName = methodInfo.DeclaringType.FullName + "." + methodInfo.Name;

                if (!string.IsNullOrEmpty(methodName))
                {
                    namesList.Add(methodName);
                }
            }

            if (flags.HasFlag(ExceptionFlags.ExceptionType))
            {
                //Get the exception type
                string exceptionType = exception.GetType().ToString();

                if (!string.IsNullOrEmpty(exceptionType))
                {
                    namesList.Add(exceptionType);
                }
            }

            string concatString = "";
            if (namesList.Count > 0)
            {
                concatString = string.Join(" - ", namesList) + ": ";
            }

            string exceptionMessage = exception.Message;

            if (exceptionMessageHandlers != null)
            {
                foreach (IExceptionMessageHandler handler in exceptionMessageHandlers)
                {
                    if (handler?.GetExceptionType() == exception.GetType())
                    {
                        exceptionMessage = handler?.GetMessage(exception) ?? exceptionMessage;
                        break;
                    }
                }
            }

            string lineNumber = "";
            if (flags.HasFlag(ExceptionFlags.Line))
            {
                //Get the line number
                int line = frame.GetFileLineNumber();

                lineNumber = $" at line {line}";
            }

            return $"{concatString}'{exceptionMessage}'{lineNumber}";
        }
    }
}