using System.IO;
using System.Windows;
using System.Windows.Media;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HaloVision.App.Models;
using HaloVision.App.Services.HalconService;
using HaloVision.App.Utils;
using Microsoft.Win32;

namespace HaloVision.App.ViewModels;

public partial class MainViewModel : ObservableObject
{
    private readonly IHalconService _halconService;
    private readonly CalibrationData _calibrationData;

    [ObservableProperty] private string imagePath = string.Empty;
    [ObservableProperty] private string status = "等待检测";
    [ObservableProperty] private double score;
    [ObservableProperty] private ImageSource? imageSource;
    [ObservableProperty] private DetectResult currentResult = new();

    public MainViewModel() : this(new HalconService())
    {
    }

    public MainViewModel(IHalconService halconService)
    {
        _halconService = halconService;
        _calibrationData = ConfigService.LoadCalibrationData();
    }

    [RelayCommand]
    private void SelectImage()
    {
        var dialog = new OpenFileDialog { Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.tif;*.tiff" };
        if (dialog.ShowDialog() != true) return;

        ImagePath = dialog.FileName;
        _halconService.LoadImage(ImagePath);
        using var img = _halconService.GetCurrentImage();
        ImageSource = ImageConverter.HObjectToBitmapSource(img);
        Status = "已选择图像，点击检测";
        InspectCommand.NotifyCanExecuteChanged();
    }

    [RelayCommand(CanExecute = nameof(CanInspect))]
    private async Task Inspect()
    {
        try
        {
            CurrentResult = await Task.Run(() => _halconService.MeasureSize(_calibrationData));
            Score = CurrentResult.IsPass ? 1.0 : 0.0;
            Status = CurrentResult.IsPass ? "OK | 检测完成" : $"NG | {CurrentResult.ErrorMsg}";
            SaveDetectResult(CurrentResult);
        }
        catch (Exception ex)
        {
            Status = $"检测异常: {ex.Message}";
            Logger.Error(Status, ex);
            MessageBox.Show(Status, "错误", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    private bool CanInspect() => !string.IsNullOrWhiteSpace(ImagePath);

    partial void OnImagePathChanged(string value) => InspectCommand.NotifyCanExecuteChanged();

    private static void SaveDetectResult(DetectResult result)
    {
        Directory.CreateDirectory("Logs");
        var line = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss},{result.IsPass},{result.Width:F2},{result.Height:F2},{result.ProcessTime.TotalMilliseconds:F0}";
        File.AppendAllText("Logs/detect_log.csv", line + Environment.NewLine);
    }
}
