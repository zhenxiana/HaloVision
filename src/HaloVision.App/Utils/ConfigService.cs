using HaloVision.App.Models;
using Newtonsoft.Json;

namespace HaloVision.App.Utils;

public static class ConfigService
{
    public static CalibrationData LoadCalibrationData(string path = "Configs/calibration.json")
    {
        try
        {
            if (!File.Exists(path)) return new CalibrationData();
            return JsonConvert.DeserializeObject<CalibrationData>(File.ReadAllText(path)) ?? new CalibrationData();
        }
        catch
        {
            return new CalibrationData();
        }
    }
}
