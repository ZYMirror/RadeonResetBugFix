namespace RadeonResetBugFixService.Tasks.BasicTasks
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Contracts;

    internal sealed class WaitForConditionTask : ITask
    {
        private readonly Func<bool> condition;
        private readonly TimeSpan timeout;
        private readonly TimeSpan pollInterval;

        public string TaskName { get; }

        public WaitForConditionTask(
            Func<bool> condition,
            TimeSpan timeout,
            TimeSpan pollInterval,
            string taskName)
        {
            this.condition = condition;
            this.timeout = timeout;
            this.pollInterval = pollInterval;
            this.TaskName = taskName;
        }

        public void Run(ILogger logger)
        {
            var stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < this.timeout)
            {
                if (this.condition())
                {
                    logger.Log($"{this.TaskName} completed after {stopwatch.ElapsedMilliseconds} ms");
                    return;
                }

                Thread.Sleep(this.pollInterval);
            }

            logger.LogError($"{this.TaskName} timed out after {stopwatch.ElapsedMilliseconds} ms");
        }
    }
}
