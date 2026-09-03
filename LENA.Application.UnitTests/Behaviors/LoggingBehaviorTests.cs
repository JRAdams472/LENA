using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using LENA.Application.Behaviors;

using MediatR;

using Microsoft.Extensions.Logging;

using Xunit;

namespace LENA.Application.UnitTests.Behaviors
{
    public class LoggingBehaviorTests
    {
        private sealed record SensitiveCommand(string Email, string Notes) : IRequest<Unit>;

        private sealed class CaptureLogger<T> : ILogger<T>
        {
            public List<(LogLevel Level, string Message)> Calls { get; } = new();

            public IDisposable? BeginScope<TState>(TState state) where TState : notnull => null;

            public bool IsEnabled(LogLevel logLevel) => true;

            public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
            {
                Calls.Add((logLevel, formatter(state, exception)));
            }
        }

        [Fact]
        public async Task Handle_Logs_Name_At_Information_And_Body_At_Debug()
        {
            var logger = new CaptureLogger<LoggingBehavior<SensitiveCommand, Unit>>();
            var behavior = new LoggingBehavior<SensitiveCommand, Unit>(logger);
            var request = new SensitiveCommand("aipaloovik@gmail.com", "secret notes");

            await behavior.Handle(request, _ => Task.FromResult(Unit.Value), CancellationToken.None);

            Assert.Contains(logger.Calls, c => c.Level == LogLevel.Information && c.Message.Contains(nameof(SensitiveCommand)));
            Assert.Contains(logger.Calls, c => c.Level == LogLevel.Debug && c.Message.Contains(request.Email));
            Assert.All(logger.Calls.Where(c => c.Level == LogLevel.Information), c => Assert.DoesNotContain(request.Email, c.Message));
        }
    }
}