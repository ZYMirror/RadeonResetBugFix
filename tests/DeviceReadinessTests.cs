namespace RadeonResetBugFixService.Tests
{
    using System;
    using System.Collections.Generic;
    using Contracts;
    using Devices;

    public static class DeviceReadinessTests
    {
        public static void Run()
        {
            ReadyBasicDisplayIsReady();
            ReadyAmdVideoIsReady();
            OtherNonAmdDisplayIsNotBasicDisplay();
            MissingFallbackVideoIsNotReady();
            DisabledAmdVideoIsNotReady();
            AmdVideoWithErrorIsNotReady();
        }

        private static void ReadyBasicDisplayIsReady()
        {
            var devices = new[] { CreateDisplayDevice("BasicDisplay", "Standard display types") };

            AssertTrue(
                DeviceReadiness.HasReadyBasicDisplay(devices),
                "A ready basic display adapter should be treated as ready");
        }

        private static void ReadyAmdVideoIsReady()
        {
            var devices = new[] { CreateDisplayDevice("amdwddmg", "Advanced Micro Devices, Inc.") };

            AssertTrue(
                DeviceReadiness.HasReadyAmdVideo(devices),
                "A ready AMD display adapter should be treated as ready");
        }

        private static void OtherNonAmdDisplayIsNotBasicDisplay()
        {
            var devices = new[] { CreateDisplayDevice("OtherDisplay", "Other vendor") };

            AssertFalse(
                DeviceReadiness.HasReadyBasicDisplay(devices),
                "A non-BasicDisplay adapter should not satisfy the basic display wait");
        }

        private static void MissingFallbackVideoIsNotReady()
        {
            var devices = new DeviceInfo[] { };

            AssertFalse(
                DeviceReadiness.HasReadyBasicDisplay(devices),
                "No display adapter should not be treated as ready");
        }

        private static void DisabledAmdVideoIsNotReady()
        {
            var device = CreateDisplayDevice("amdwddmg", "Advanced Micro Devices, Inc.");
            device.ErrorCode = 22;
            var devices = new[] { device };

            AssertFalse(
                DeviceReadiness.HasReadyAmdVideo(devices),
                "A disabled AMD display adapter should not be treated as ready");
        }

        private static void AmdVideoWithErrorIsNotReady()
        {
            var device = CreateDisplayDevice("amdwddmg", "Advanced Micro Devices, Inc.");
            device.ErrorCode = 1;
            var devices = new[] { device };

            AssertFalse(
                DeviceReadiness.HasReadyAmdVideo(devices),
                "An AMD display adapter with an error should not be treated as ready");
        }

        private static DeviceInfo CreateDisplayDevice(string service, string manufacturer)
        {
            return new DeviceInfo
            {
                ClassName = "Display",
                DeviceId = "PCI\\TEST",
                ErrorCode = 0,
                IsPresent = true,
                Manufacturer = manufacturer,
                Name = service,
                Service = service,
            };
        }

        private static void AssertTrue(bool condition, string message)
        {
            if (!condition)
            {
                throw new InvalidOperationException(message);
            }
        }

        private static void AssertFalse(bool condition, string message)
        {
            if (condition)
            {
                throw new InvalidOperationException(message);
            }
        }
    }
}
