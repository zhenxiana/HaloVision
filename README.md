# HaloVision

C# + WPF + MVVM + HALCON 机器视觉示例（.NET 6 / x64）。

## 本次按“全流程文档”补齐的内容

- 补充了更完整的 Model 层：`DetectResult`、`DefectInfo`、`CalibrationData`、`ImageInfo`。
- 新增 `Services/HalconService` 分层：`IHalconService` + `HalconService`，并采用 `MeanImage -> Threshold -> OpeningCircle -> SmallestRectangle2/AreaCenter` 的测量流程。
- 新增 `Utils` 工具层：`ImageConverter`（Halcon 转 WPF 图像）、`ConfigService`（标定参数加载）、`Logger`（NLog 封装）。
- 新增 `Configs/calibration.json` 并配置为输出目录自动复制。
- 保留原有 `IVisionService/HalconVisionService` 以兼容既有界面调用，避免一次性大改造成 UI 断裂。

## 当前目录结构

```text
src/HaloVision.App
├─ Configs
├─ Models
├─ Services
│  ├─ HalconService
│  ├─ HalconVisionService.cs
│  └─ IVisionService.cs
├─ Utils
├─ ViewModels
└─ Views
```

## 说明

- HALCON .NET 绑定在不同版本下重载差异较大，本项目优先采用 `HOperatorSet` 显式调用与 `HTuple` 参数，减少二义性编译错误。
