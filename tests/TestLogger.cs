namespace RadeonResetBugFixService.Tests
{
    using System;
    using System.Collections.Generic;
    using Contracts;

    public sealed class TestLogger : ILogger
    {
        public IList<string> Messages { get; } = new List<string>();

        public void Log(string message)
        {
            this.Messages.Add(message);
        }

        public void LogError(string message)
        {
            this.Messages.Add(message);
        }

        public void Dispose()
        {
        }
    }
}
