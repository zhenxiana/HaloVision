using System.Diagnostics;
using HaloVision.App.Models;
using HaloVision.App.Utils;
using HalconDotNet;

namespace HaloVision.App.Services.HalconService;

public class HalconService : IHalconService
{
    private HObject _currentImage;
    private bool _disposed;

    public HalconService()
    {
        HOperatorSet.GenEmptyObj(out _currentImage);
    }

    public void LoadImage(string path)
    {
        HOperatorSet.ClearObj(_currentImage);
        HOperatorSet.ReadImage(out _currentImage, path);
    }

    public void SetImage(HObject image)
    {
        HOperatorSet.ClearObj(_currentImage);
        _currentImage = image.CopyObj(1, -1);
    }

    public HObject GetCurrentImage() => _currentImage.CopyObj(1, -1);

    public DetectResult MeasureSize(CalibrationData calibration)
    {
        var result = new DetectResult();
        var sw = Stopwatch.StartNew();

        try
        {
            HOperatorSet.MeanImage(_currentImage, out HObject imageSmooth, 3, 3);
            HOperatorSet.Threshold(imageSmooth, out HObject region, new HTuple(100), new HTuple(255));
            HOperatorSet.OpeningCircle(region, out HObject regionOpening, 3.5);

            HOperatorSet.AreaCenter(regionOpening, out _, out HTuple row, out HTuple col);
            HOperatorSet.SmallestRectangle2(regionOpening, out _, out _, out HTuple angle, out HTuple length1, out HTuple length2);

            result.Width = length1.D * 2 * calibration.PixelToMmRatio;
            result.Height = length2.D * 2 * calibration.PixelToMmRatio;
            result.CenterX = col.D * calibration.PixelToMmRatio + calibration.OffsetX;
            result.CenterY = row.D * calibration.PixelToMmRatio + calibration.OffsetY;
            result.Angle = angle.D * 180 / Math.PI;
            result.IsPass = true;

            HOperatorSet.ClearObj(imageSmooth);
            HOperatorSet.ClearObj(region);
            HOperatorSet.ClearObj(regionOpening);
        }
        catch (Exception ex)
        {
            result.IsPass = false;
            result.ErrorMsg = ex.Message;
            Logger.Error($"尺寸测量失败：{ex.Message}", ex);
        }
        finally
        {
            sw.Stop();
            result.ProcessTime = sw.Elapsed;
        }

        return result;
    }

    public void Dispose()
    {
        if (_disposed) return;
        HOperatorSet.ClearObj(_currentImage);
        _disposed = true;
        GC.SuppressFinalize(this);
    }
}
