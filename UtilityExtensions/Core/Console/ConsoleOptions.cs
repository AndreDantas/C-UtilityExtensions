using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityExtensions.Core.Console
{
    public struct ConsoleOptions
    {
        public ConsoleColor textColor;
        public ConsoleColor backgroundColor;

        public static ConsoleOptions GetCurrent()
        {
            return new ConsoleOptions
            {
                backgroundColor = System.Console.BackgroundColor,
                textColor = System.Console.ForegroundColor
            };
        }
    }
}