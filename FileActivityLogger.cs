using System;
using System.IO;

public class FileActivityLogger : IActivityLogger
{
    private readonly string filePath;

    public FileActivityLogger(string filePath = "Activity.txt")
    {
        this.filePath = filePath;
    }

    public void LogActivity(string action, bool performed, string role, string timestamp)
    {
        try
        {
            string logEntry = $"\n{action},{performed},{role},{timestamp}";
            File.AppendAllText(filePath, logEntry);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error logging activity: {ex.Message}");
        }
    }
} 