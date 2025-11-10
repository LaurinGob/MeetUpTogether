using System.IO;
using System.Text.Json;

namespace MeetUpTogether.BLL.Services
{
    public class AppSettings
    {
        public PathSettings Paths { get; set; } = new();

        public static AppSettings Load(string filePath = "appsettings.json")
        {
            var basePath = AppContext.BaseDirectory;
            var fullPath = Path.Combine(basePath, filePath);

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"Configuration file not found: {fullPath}");

            var json = File.ReadAllText(fullPath);
            return JsonSerializer.Deserialize<AppSettings>(json)
                   ?? new AppSettings();
        }
    }

    public class PathSettings
    {
        public string ReportsFolder { get; set; } = "reports";
        public string LogsFolder { get; set; } = "logs";
    }
}
