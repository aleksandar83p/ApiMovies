using ApiMovies.Controllers;
using Microsoft.Extensions.Logging;
using System;

namespace ApiMovies.Logger
{
    public class ExceptionLogger: ILogger
    {
        public ILogger<BaseController> Logger { get; }
        public ExceptionLogger(ILogger<BaseController> logger)
        {
            Logger = logger;
        }

        public IDisposable BeginScope<TState>(TState state)
        {
            return Logger.BeginScope(state);
        }

        public bool IsEnabled(LogLevel logLevel)
        {
            return Logger.IsEnabled(logLevel);
        }

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            Logger.Log(logLevel, eventId, state, exception, formatter);
        }
    }
}
