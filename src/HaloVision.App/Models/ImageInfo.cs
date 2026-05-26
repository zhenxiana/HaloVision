namespace HaloVision.App.Models;

public class ImageInfo
{
    public string FilePath { get; set; } = string.Empty;
    public int Width { get; set; }
    public int Height { get; set; }
    public DateTime CaptureTime { get; set; }
    public string CameraId { get; set; } = string.Empty;
}
