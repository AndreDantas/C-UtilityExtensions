using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityExtensions.Core.Console.Loggers
{
    public interface ILogger
    {
        string Log(string message);
    }
}