using System.Text.Json;
using DustInTheWind.OroWpf.Ports.SettingsAccess.Models;

namespace DustInTheWind.OroWpf.Ports.SettingsAccess;

internal class SettingsStorage
{
    private readonly Lock synchronizationObject = new();
    private readonly Timer timer;
    private AppSettings appSettings;

    private readonly Lazy<JsonSerializerOptions> serializerOptions = new(() =>
    {
        JsonSerializerOptions jsonSerializerOptions = new()
        {
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
            WriteIndented = true
        };

        return jsonSerializerOptions;
    });

    public TimeSpan Delay { get; set; } = TimeSpan.FromMilliseconds(100);

    public SettingsStorage()
    {
        timer = new Timer(HandleTimerTick);
    }

    public void Save(AppSettings appSettings)
    {
        ArgumentNullException.ThrowIfNull(appSettings);

        lock (synchronizationObject)
        {
            this.appSettings = appSettings;
            timer.Change(Delay, Timeout.InfiniteTimeSpan);
        }
    }

    private void HandleTimerTick(object state)
    {
        lock (synchronizationObject)
        {
            string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

            string outputJson = JsonSerializer.Serialize(appSettings, serializerOptions.Value);
            File.WriteAllText(filePath, outputJson);
        }
    }
}
