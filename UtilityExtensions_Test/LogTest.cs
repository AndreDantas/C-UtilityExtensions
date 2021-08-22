using NUnit.Framework;
using System;
using System.Collections.Generic;
using UtilityExtensions.Core;

namespace UtilityExtensions_Test
{
    public class LogTest
    {
        [Test]
        public void ExceptionLog()
        {
            string nullstring = null;

            string log = "";
            try
            {
                nullstring.ToLower();
            }
            catch (Exception e)
            {
                log = Log.Exception(e);
            }

            Assert.IsNotEmpty(log);
        }

        [Test]
        public void ExceptionLog_MessageHandlers()
        {
            string nullstring = null;

            string log = "";
            try
            {
                nullstring.ToLower();
            }
            catch (Exception e)
            {
                log = Log.Exception(e, exceptionMessageHandlers:
                    new List<Log.IExceptionMessageHandler> {
                        new Log.ExceptionMessageHandler<ArgumentNullException>((e) => "{ArgumentNullException}"),
                        new Log.ExceptionMessageHandler<NullReferenceException>((e) => "{NullReferenceException}")
                    });
            }

            Assert.False(log.Contains("{ArgumentNullException}"));
            Assert.True(log.Contains("{NullReferenceException}"));
        }
    }
}