# HaloVision

一个基于 **C# + WPF + MVVM** 的 Halcon 机器视觉示例项目。

## 项目结构

```text
HaloVision.sln
└── src/HaloVision.App
    ├── Models            # 检测结果模型
    ├── Services          # Halcon 视觉算法封装
    ├── ViewModels        # MVVM 逻辑层
    └── Views             # WPF 视图
```

## 功能说明

- 选择本地图像。
- 调用 Halcon 做 Canny 边缘提取。
- 根据边缘面积输出 OK/NG 和置信度分数。

## 运行前准备

1. 安装 HALCON（并确保配置 `HALCONROOT` 环境变量）。
2. 确认 `halcondotnet.dll` 路径有效：
   - `$(HALCONROOT)\\bin\\dotnet35\\halcondotnet.dll`
3. 使用 Visual Studio 2022+ 或 .NET 8 SDK 构建。

## 关键代码

- `MainViewModel`：负责 UI 命令与状态管理。
- `HalconVisionService`：封装 Halcon 图像处理流程。
- `VisionResult`：承载检测结果。

## 后续可扩展

- 增加相机实时采集（GigE / USB3 Vision）。
- 把阈值参数放到配置界面。
- 增加多工位、多配方管理。
