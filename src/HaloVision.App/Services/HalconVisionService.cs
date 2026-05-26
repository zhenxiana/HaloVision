using HaloVision.App.Models;
using HalconDotNet;

namespace HaloVision.App.Services;

public class HalconVisionService : IVisionService
{
    public VisionResult RunEdgeInspection(string imagePath)
    {
        try
        {
            using var image = new HImage(imagePath);
            using var gray = image.Rgb1ToGray();

            HOperatorSet.MeanImage(gray, out HObject gauss, 3, 3);
            HOperatorSet.Threshold(gauss, out HObject region, new HTuple(100), new HTuple(255));
            HOperatorSet.OpeningCircle(region, out HObject opening, 3.5);

            HOperatorSet.AreaCenter(opening, out HTuple area, out _, out _);
            var score = Math.Min(1.0, area.D / 10000.0);

            HOperatorSet.ClearObj(gauss);
            HOperatorSet.ClearObj(region);
            HOperatorSet.ClearObj(opening);

            return new VisionResult(score > 0.15, score, $"区域面积: {area.D:F0}");
        }
        catch (Exception ex)
        {
            return new VisionResult(false, 0, $"检测失败: {ex.Message}");
        }
    }
}
