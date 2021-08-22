using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityExtensions.Core.Console.Loggers
{
    /// <summary>
    /// Default logger, returns the given message
    /// </summary>
    public sealed class DefaultLogger : ILogger
    {
        public string Log(string message)
        {
            return message;
        }
    }
}