using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityExtensions.Core.Console.Loggers
{
    /// <summary>
    /// Logs the message with the current time
    /// </summary>
    public class TimeLogger : ILogger
    {
        public string format = "yyyy/MM/dd HH:mm:ss";

        public TimeLogger()
        {
        }

        public TimeLogger(string format)
        {
            this.format = format;
        }

        public string Log(string message)
        {
            return $"{DateTime.Now.ToString(format)} - {message}";
        }
    }
}