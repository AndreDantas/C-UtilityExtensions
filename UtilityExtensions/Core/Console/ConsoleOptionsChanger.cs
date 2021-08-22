using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UtilityExtensions.Core.Console
{
    public class ConsoleOptionsChanger : IDisposable
    {
        private ConsoleOptions old;
        private ConsoleOptions @new;

        public ConsoleOptionsChanger()
        {
            old = ConsoleOptions.GetCurrent();
        }

        public ConsoleOptionsChanger(ConsoleOptions options) : base()
        {
            ChangeOptions(options);
        }

        public void ChangeOptions(ConsoleOptions options)
        {
            @new = options;
            SetConsoleOptions(@new);
        }

        private void SetConsoleOptions(ConsoleOptions options)
        {
            System.Console.ForegroundColor = options.textColor;
            System.Console.BackgroundColor = options.backgroundColor;
        }

        public void Dispose()
        {
            SetConsoleOptions(old);
        }
    }
}