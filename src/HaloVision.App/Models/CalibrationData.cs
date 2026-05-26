namespace HaloVision.App.Models;

public class CalibrationData
{
    public double PixelToMmRatio { get; set; } = 0.05;
    public double OffsetX { get; set; }
    public double OffsetY { get; set; }
}
