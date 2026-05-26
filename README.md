# HaloVision

C# + WPF + MVVM + Halcon 机器视觉开发示例（按 .NET 6 / x64 进行工程化调整）。

## 本次修复重点

- 修复你反馈的编译错误：移除了 `EdgesImage("canny", ... )` 这条不匹配重载的调用链。
- 新增 `IHalconService/HalconService`，改用 `GaussFilter + Threshold + OpeningCircle + SmallestRectangle2` 的尺寸测量流程。
- 切换为 `CommunityToolkit.Mvvm`（`ObservableObject` + `[RelayCommand]`）。
- 项目切换到 `net6.0-windows`、`x64`，并补充文档要求的 NuGet 依赖。
- UI 改为 `MahApps.Metro` 风格窗口并展示检测结果字段。

## 目录结构

```text
src/HaloVision.App
├─ Models
├─ Services/HalconService
├─ Utils
├─ Configs
├─ ViewModels
└─ Views
```
