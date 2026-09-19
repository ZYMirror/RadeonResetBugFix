namespace RadeonResetBugFixService.Tests
{
    using System;

    public static class TestProgram
    {
        public static int Main()
        {
            DeviceReadinessTests.Run();
            WaitForConditionTaskTests.Run();
            Console.WriteLine("All tests passed");
            return 0;
        }
    }
}
