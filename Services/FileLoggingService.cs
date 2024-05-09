using CANSAT_UI.Models;
using CANSAT_UI.Services.Contracts;
using System.IO;

namespace CANSAT_UI.Services;

public class FileLoggingService : ILoggingService
{
    private readonly string _logFilePath;

    public FileLoggingService(string logFilePath)
    {
        _logFilePath = logFilePath;
    }

    public void Log(LogEntry logEntry)
    {
        string logLine = $"{logEntry.Timestamp} [{logEntry.Level}] - {logEntry.Message}\n";
        try
        {
            using StreamWriter writer = new(_logFilePath, true);
            writer.WriteLine(logLine);
        }
        catch
        {
            throw;
        }
    }

}
