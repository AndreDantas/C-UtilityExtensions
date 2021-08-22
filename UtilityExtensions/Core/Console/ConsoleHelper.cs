using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UtilityExtensions.Extensions;
using UtilityExtensions.Core.Console.Loggers;

namespace UtilityExtensions.Core.Console
{
    public sealed class ConsoleHelper
    {
        private ConsoleOptionsChanger changer = new ConsoleOptionsChanger();
        private ILogger logger = new DefaultLogger();
        private ConsoleOptions options;
        private LimitedQueue<string> logs = new LimitedQueue<string>();
        private int logHistorySize = int.MaxValue / 2;
        public IReadOnlyCollection<string> Logs => logs.ToList().AsReadOnly();

        public ConsoleHelper()
        {
            options = ConsoleOptions.GetCurrent();
            logs.Limit = logHistorySize;
        }

        public ConsoleHelper(ILogger logger = null, ConsoleOptions options = default, int logHistorySize = int.MaxValue / 2)
        {
            this.logger = logger ?? new DefaultLogger();
            this.options = options;
            this.logHistorySize = logHistorySize;
            logs.Limit = logHistorySize;

            ChangeConsoleOptions(this.options);
        }

        public void Write(string message)
        {
            System.Console.Write(LogMessage(message));
        }

        public void Write(string message, ConsoleOptions options)
        {
            Write(message, options, logger);
        }

        public void Write(string message, ConsoleOptions options, ILogger logger)
        {
            WhileUsingOptions(() => Write(message), options, logger);
        }

        public void Write(string message, ConsoleColor textColor)
        {
            var opt = options;
            opt.textColor = textColor;
            Write(message, opt);
        }

        public void Write(string message, ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            var opt = options;
            opt.textColor = textColor;
            opt.backgroundColor = backgroundColor;
            Write(message, opt);
        }

        public void WriteLine(string message)
        {
            System.Console.WriteLine(LogMessage(message));
        }

        public void WriteLine(string message, ConsoleOptions options)
        {
            WriteLine(message, options, logger);
        }

        public void WriteLine(string message, ConsoleOptions options, ILogger logger)
        {
            WhileUsingOptions(() => WriteLine(message), options, logger);
        }

        public void WriteLine(string message, ConsoleColor textColor)
        {
            var opt = options;
            opt.textColor = textColor;
            WriteLine(message, opt);
        }

        public void WriteLine(string message, ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            var opt = options;
            opt.textColor = textColor;
            opt.backgroundColor = backgroundColor;
            WriteLine(message, opt);
        }

        public void ReplaceLine(string message)
        {
            DeleteLine();
            WriteLine(message);
        }

        public void ReplaceLine(string message, ConsoleOptions options)
        {
            ReplaceLine(message, options, logger);
        }

        public void ReplaceLine(string message, ConsoleOptions options, ILogger logger)
        {
            WhileUsingOptions(() => ReplaceLine(message), options, logger);
        }

        public void ReplaceLine(string message, ConsoleColor textColor)
        {
            var opt = options;
            opt.textColor = textColor;
            ReplaceLine(message, opt);
        }

        public void ReplaceLine(string message, ConsoleColor textColor, ConsoleColor backgroundColor)
        {
            var opt = options;
            opt.textColor = textColor;
            opt.backgroundColor = backgroundColor;
            ReplaceLine(message, opt);
        }

        public ConsoleKeyInfo ReadKey()
        {
            return System.Console.ReadKey();
        }

        public int Read()
        {
            return System.Console.Read();
        }

        public string ReadLine()
        {
            return System.Console.ReadLine();
        }

        public void DeleteLine()
        {
            if (logs.Count == 0)
                return;

            System.Console.SetCursorPosition(0, System.Console.CursorTop - 1);
            System.Console.Write(new string(' ', logs.LastOrDefault("").Length));
            System.Console.SetCursorPosition(0, System.Console.CursorTop);
        }

        public void SetLogger(ILogger logger)
        {
            this.logger = logger;
        }

        public void SetOptions(ConsoleOptions options)
        {
            this.options = options;
            ChangeConsoleOptions(options);
        }

        private void WhileUsingOptions(Action action, ConsoleOptions options, ILogger logger)
        {
            ChangeConsoleOptions(options);
            var currentLogger = this.logger;
            SetLogger(logger);
            action?.Invoke();
            SetLogger(currentLogger);
            ChangeConsoleOptions(this.options);
        }

        private void ChangeConsoleOptions(ConsoleOptions options)
        {
            changer.ChangeOptions(options);
        }

        private string LogMessage(string message)
        {
            var log = logger?.Log(message);
            logs.Enqueue(log);
            return log;
        }
    }
}