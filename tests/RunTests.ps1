$ErrorActionPreference = 'Stop'

$csc = 'C:\Users\H2O2\.nuget\packages\microsoft.net.compilers.toolset\4.0.1\tasks\net472\csc.exe'
$output = Join-Path $PSScriptRoot 'obj\RadeonResetBugFixService.Tests.exe'
$outputDir = Split-Path $output -Parent
New-Item -ItemType Directory -Force -Path $outputDir | Out-Null

$sources = @(
    (Join-Path $PSScriptRoot '..\RadeonResetBugFixService\Contracts\DeviceInfo.cs'),
    (Join-Path $PSScriptRoot '..\RadeonResetBugFixService\Contracts\ILogger.cs'),
    (Join-Path $PSScriptRoot '..\RadeonResetBugFixService\Devices\KnownDevices.cs'),
    (Join-Path $PSScriptRoot '..\RadeonResetBugFixService\Devices\DeviceReadiness.cs'),
    (Join-Path $PSScriptRoot '..\RadeonResetBugFixService\Tasks\ITask.cs'),
    (Join-Path $PSScriptRoot '..\RadeonResetBugFixService\Tasks\BasicTasks\WaitForConditionTask.cs'),
    (Join-Path $PSScriptRoot 'DeviceReadinessTests.cs'),
    (Join-Path $PSScriptRoot 'TestLogger.cs'),
    (Join-Path $PSScriptRoot 'TestProgram.cs'),
    (Join-Path $PSScriptRoot 'WaitForConditionTaskTests.cs')
)

& $csc /nologo /target:exe /out:$output /r:System.dll /r:System.Core.dll $sources
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}

& $output
if ($LASTEXITCODE -ne 0) {
    exit $LASTEXITCODE
}
