using System;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace Data.Provider
{
    public class SqlLoggerProvider : ILoggerProvider
    {
        public void Dispose()
        {
        }

        public ILogger CreateLogger(string categoryName)
        {
            return new SqlLoger();
        }

        private class SqlLoger : ILogger
        {
            public IDisposable BeginScope<TState>(TState state)
            {
                return null;// throw new NotImplementedException();
            }

            public bool IsEnabled(LogLevel logLevel)
            {
                return true;
            }

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
            {
                //throw new NotImplementedException();
                Debug.WriteLine($"SQL: ${formatter(state, exception)}");
            }
        }
    }
}