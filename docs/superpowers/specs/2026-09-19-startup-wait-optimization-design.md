# 启动等待优化设计

## 目标

把 `StartupTask` 中的两段固定等待改成条件等待，同时保留原超时时间作为兜底：

- 第一段 40 秒等待：等待 `BasicDisplay` 兜底显示驱动进入可用状态。
- 第二段 20 秒等待：等待 AMD 显示适配器进入可用状态。

## 方案

1. 新增 `DeviceReadiness`，只判断显示适配器是否满足：
   - `Present == true`
   - `ConfigManagerErrorCode == 0`
   - 未处于禁用状态
2. 新增 `WaitForConditionTask`，按固定间隔轮询条件，条件满足立即返回，超时后继续执行后续任务。
3. `DeviceHelper` 增加只查询 `PNPClass='Display'` 的方法，避免每次轮询都枚举全部 PnP 设备。
4. `StartupTask` 用新的条件等待替换两处 `SleepTask`。

## VM 证据

在目标 Win10 虚拟机上：

- `Microsoft Basic Display Adapter` 存在，`ConfigManagerErrorCode` 为 `0`，状态为 `OK`。
- 服务启动日志中，`Enabling basic display automatic start` 紧接在第一段等待之前。
- 因此第一段等待应等待 `BasicDisplay` 就绪，而不是任意非 AMD 适配器。

## 不做的事

- 不改变关机流程。
- 不改变设备启用/禁用顺序。
- 不尝试检测显示器拓扑；在服务会话中该信息不可靠。
- 不移除兜底超时，异常情况下仍保留原版最长时间。

## 验证

- 单元测试覆盖：
  - 非 AMD 显示适配器就绪判定
  - AMD 显示适配器就绪判定
  - 设备缺失、禁用、错误码非 0 时的排除
  - 条件等待的成功、超时和轮询行为
- Release 构建通过。
