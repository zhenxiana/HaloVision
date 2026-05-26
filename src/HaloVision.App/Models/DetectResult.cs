namespace HaloVision.App.Models;

public class DetectResult
{
    public bool IsPass { get; set; }
    public double Width { get; set; }
    public double Height { get; set; }
    public double CenterX { get; set; }
    public double CenterY { get; set; }
    public double Angle { get; set; }
    public List<DefectInfo> Defects { get; set; } = new();
    public string ErrorMsg { get; set; } = string.Empty;
    public TimeSpan ProcessTime { get; set; }
}
