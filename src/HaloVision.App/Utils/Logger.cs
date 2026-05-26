using NLog;

namespace HaloVision.App.Utils;

public static class Logger
{
    private static readonly NLog.Logger Log = LogManager.GetCurrentClassLogger();

    public static void Info(string message) => Log.Info(message);

    public static void Error(string message, Exception? ex = null)
    {
        if (ex is null) Log.Error(message);
        else Log.Error(ex, message);
    }
}
