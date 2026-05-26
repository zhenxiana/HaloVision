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
            using var edges = gray.EdgesImage("canny", 1.2, 20, 40);
            var area = edges.AreaCenter(out _, out _);

            var score = Math.Min(1.0, area.D / 10000.0);
            return new VisionResult(score > 0.15, score, $"边缘面积: {area.D:F0}");
        }
        catch (Exception ex)
        {
            return new VisionResult(false, 0, $"检测失败: {ex.Message}");
        }
    }
}
