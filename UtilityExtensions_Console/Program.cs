using System;
using System.Threading;
using UtilityExtensions.Extensions;
using UtilityExtensions.Core.Console;
using UtilityExtensions.Core.Console.Loggers;

namespace UtilityExtensions_Console
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var c = new ConsoleHelper(new TimeLogger(), new ConsoleOptions { textColor = ConsoleColor.Blue, backgroundColor = ConsoleColor.White });

            c.WriteLine("Test");
            Thread.Sleep(200);
            c.Write("Testing error: ");
            Thread.Sleep(200);
            c.WriteLine(" RED ERROR TEST ", new ConsoleOptions { textColor = ConsoleColor.Red }, new DefaultLogger());
            Thread.Sleep(200);
            c.WriteLine($"Loading test 0%");
            for (int i = 0; i <= 100; i++)
            {
                c.ReplaceLine($"Loading test {i}%", EnumExtensions.RandomEnumValue<ConsoleColor>());
                Thread.Sleep(50);
            }
            c.ReadKey();
        }
    }
}