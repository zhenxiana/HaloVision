using System.Windows.Input;
using HaloVision.App.Services;
using Microsoft.Win32;

namespace HaloVision.App.ViewModels;

public class MainViewModel : ViewModelBase
{
    private readonly IVisionService _visionService;
    private string _imagePath = string.Empty;
    private string _status = "等待检测";
    private double _score;

    public MainViewModel() : this(new HalconVisionService())
    {
    }

    public MainViewModel(IVisionService visionService)
    {
        _visionService = visionService;
        SelectImageCommand = new RelayCommand(SelectImage);
        InspectCommand = new RelayCommand(Inspect, () => !string.IsNullOrWhiteSpace(ImagePath));
    }

    public string ImagePath
    {
        get => _imagePath;
        set
        {
            SetProperty(ref _imagePath, value);
            (InspectCommand as RelayCommand)?.RaiseCanExecuteChanged();
        }
    }

    public string Status
    {
        get => _status;
        private set => SetProperty(ref _status, value);
    }

    public double Score
    {
        get => _score;
        private set => SetProperty(ref _score, value);
    }

    public ICommand SelectImageCommand { get; }
    public ICommand InspectCommand { get; }

    private void SelectImage()
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;*.tif;*.tiff"
        };

        if (dialog.ShowDialog() == true)
        {
            ImagePath = dialog.FileName;
            Status = "已选择图像，点击检测";
        }
    }

    private void Inspect()
    {
        var result = _visionService.RunEdgeInspection(ImagePath);
        Score = result.Score;
        Status = $"{(result.IsOk ? "OK" : "NG")} | {result.Message}";
    }
}
