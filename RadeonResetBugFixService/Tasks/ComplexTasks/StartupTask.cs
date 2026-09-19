namespace RadeonResetBugFixService.Tasks.ComplexTasks
{
    using BasicTasks;
    using Contracts;
    using Devices;
    using System;

    class StartupTask : AbstractSequentialTask
    {
        public StartupTask(ServiceContext context)
        {
            this.Context = context;
        }

        private ServiceContext Context { get; }

        public override string TaskName => "Startup";

        protected override ITask[] Subtasks => new ITask[]
        {
            new EnableBasicDisplayStartupTask(),
            new WaitForConditionTask(
                () => DeviceReadiness.HasReadyBasicDisplay(DeviceHelper.GetDisplayDevices()),
                TimeSpan.FromSeconds(40),
                TimeSpan.FromSeconds(1),
                "Waiting for basic display to become ready"),
            new EnableAmdVideoTask(this.Context.StartupDevicesStatus),
            new DisableVirtualVideoTask(this.Context.StartupDevicesStatus),
            new WaitForConditionTask(
                () => DeviceReadiness.HasReadyAmdVideo(DeviceHelper.GetDisplayDevices()),
                TimeSpan.FromSeconds(20),
                TimeSpan.FromSeconds(1),
                "Waiting for AMD video to become ready"),
            new FixMonitorTask(),
            new DisableVirtualVideoTask(this.Context.StartupDevicesStatus),
            new FixMonitorTask()
        };
    }
}
