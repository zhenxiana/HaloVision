using HaloVision.App.Models;
using HalconDotNet;

namespace HaloVision.App.Services.HalconService;

public interface IHalconService : IDisposable
{
    void LoadImage(string path);
    void SetImage(HObject image);
    HObject GetCurrentImage();
    DetectResult MeasureSize(CalibrationData calibration);
}
