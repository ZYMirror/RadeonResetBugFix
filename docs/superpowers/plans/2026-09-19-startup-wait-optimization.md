# 启动等待优化实现计划

> **面向 AI 代理的工作者：** 必需子技能：使用 superpowers:executing-plans 逐任务实现此计划。步骤使用复选框（`- [ ]`）语法来跟踪进度。

**目标：** 让 `StartupTask` 在显示适配器就绪后提前结束等待，而不是固定睡满 40 秒和 20 秒。

**架构：** 新增 `DeviceReadiness` 和 `WaitForConditionTask`，并在 `DeviceHelper` 中提供只查询显示适配器的方法。`StartupTask` 保留原任务顺序，仅替换两处固定等待。

**技术栈：** .NET Framework 4.7.2、Windows Service、C#、独立测试可执行文件。

---

### 任务 1：实现显示适配器就绪判定

**文件：**
- 创建：`RadeonResetBugFixService/Devices/DeviceReadiness.cs`
- 修改：`RadeonResetBugFixService/RadeonResetBugFixService.csproj`
- 创建：`tests/DeviceReadinessTests.cs`
- 修改：`tests/RunTests.ps1`
- 修改：`tests/TestProgram.cs`

- [x] 编写 `DeviceReadinessTests`，覆盖：
  - `BasicDisplay` 就绪
  - AMD 显示适配器就绪
  - 设备缺失、禁用、错误码非 0 时不判定为就绪
- [x] 运行测试，确认因 `DeviceReadiness` 不存在而失败。
- [x] 实现 `DeviceReadiness`。
- [x] 运行测试，确认全部通过。

### 任务 2：实现条件等待任务

**文件：**
- 创建：`RadeonResetBugFixService/Tasks/BasicTasks/WaitForConditionTask.cs`
- 修改：`RadeonResetBugFixService/RadeonResetBugFixService.csproj`
- 创建：`tests/WaitForConditionTaskTests.cs`
- 修改：`tests/RunTests.ps1`
- 修改：`tests/TestProgram.cs`

- [x] 编写 `WaitForConditionTaskTests`，覆盖：
  - 条件立即满足时不再等待
  - 条件延迟满足时提前结束
  - 条件始终不满足时按超时返回
- [x] 运行测试，确认因 `WaitForConditionTask` 不存在而失败。
- [x] 实现 `WaitForConditionTask`。
- [x] 运行测试，确认全部通过。

### 任务 3：接入启动流程

**文件：**
- 修改：`RadeonResetBugFixService/Devices/DeviceHelper.cs`
- 修改：`RadeonResetBugFixService/Tasks/ComplexTasks/StartupTask.cs`

- [x] 在 `DeviceHelper` 中新增 `GetDisplayDevices()`，使用 WQL 过滤 `PNPClass='Display'`。
- [x] 将第一段 `SleepTask(40s)` 替换为等待“`BasicDisplay` 就绪”。
- [x] 将第二段 `SleepTask(20s)` 替换为等待“AMD 显示适配器就绪”。
- [x] 运行全部单元测试。
- [x] 构建 Release 版本。
