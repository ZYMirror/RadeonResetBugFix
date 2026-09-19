namespace RadeonResetBugFixService.Devices
{
    using System.Collections.Generic;
    using System.Linq;
    using Contracts;

    internal static class DeviceReadiness
    {
        public static bool HasReadyBasicDisplay(IEnumerable<DeviceInfo> devices)
        {
            return devices.Any(device => KnownDevices.IsBasicDisplay(device) && IsReady(device));
        }

        public static bool HasReadyAmdVideo(IEnumerable<DeviceInfo> devices)
        {
            return devices.Any(device => KnownDevices.IsAmdVideo(device) && IsReady(device));
        }

        private static bool IsReady(DeviceInfo device)
        {
            return device.IsPresent && !device.IsDisabled && device.ErrorCode == 0;
        }
    }
}
