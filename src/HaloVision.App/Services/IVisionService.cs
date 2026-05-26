using HaloVision.App.Models;

namespace HaloVision.App.Services;

public interface IVisionService
{
    VisionResult RunEdgeInspection(string imagePath);
}
