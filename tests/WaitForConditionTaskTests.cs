namespace RadeonResetBugFixService.Tests
{
    using System;
    using System.Diagnostics;
    using System.Threading;
    using Tasks.BasicTasks;

    public static class WaitForConditionTaskTests
    {
        public static void Run()
        {
            ImmediateConditionCompletesWithoutWaiting();
            DelayedConditionCompletesEarly();
            TimeoutIsRespectedWhenConditionNeverCompletes();
        }

        private static void ImmediateConditionCompletesWithoutWaiting()
        {
            var task = new WaitForConditionTask(
                () => true,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromMilliseconds(10),
                "Immediate condition");

            using (var logger = new TestLogger())
            {
                var stopwatch = Stopwatch.StartNew();
                task.Run(logger);
                stopwatch.Stop();

                AssertTrue(stopwatch.ElapsedMilliseconds < 100, "Immediate condition should not wait");
            }
        }

        private static void DelayedConditionCompletesEarly()
        {
            var attempts = 0;
            var task = new WaitForConditionTask(
                () => Interlocked.Increment(ref attempts) >= 3,
                TimeSpan.FromSeconds(1),
                TimeSpan.FromMilliseconds(10),
                "Delayed condition");

            using (var logger = new TestLogger())
            {
                var stopwatch = Stopwatch.StartNew();
                task.Run(logger);
                stopwatch.Stop();

                AssertTrue(attempts >= 3, "Delayed condition should be retried until it becomes true");
                AssertTrue(stopwatch.ElapsedMilliseconds < 500, "Delayed condition should finish before the timeout");
            }
        }

        private static void TimeoutIsRespectedWhenConditionNeverCompletes()
        {
            var task = new WaitForConditionTask(
                () => false,
                TimeSpan.FromMilliseconds(30),
                TimeSpan.FromMilliseconds(10),
                "Never-ready condition");

            using (var logger = new TestLogger())
            {
                var stopwatch = Stopwatch.StartNew();
                task.Run(logger);
                stopwatch.Stop();

                AssertTrue(stopwatch.ElapsedMilliseconds >= 20, "Timeout should be respected");
                AssertTrue(logger.Messages.Count > 0, "Timeout should be logged");
            }
        }

        private static void AssertTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
