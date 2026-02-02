using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace DustInTheWind.OroWpf.Ports.SettingsAccess;

public class Settings : ISettings
{
    private const string KeepOnTopKey = "KeepOnTop";
    private const string WindowLeftKey = "WindowLeft";
    private const string WindowTopKey = "WindowTop";
    private readonly IConfigurationRoot configuration;

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

    public Settings()
    {
        configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true)
            .Build();
    }

    public bool KeepOnTop
    {
        get
        {
            string rawValue = configuration[KeepOnTopKey];

            if (rawValue == null)
                return true;

            bool success = bool.TryParse(rawValue, out bool value);
            return success ? value : true;
        }
        set
        {
            configuration[KeepOnTopKey] = value.ToString();
            Save();
            OnKeepOnTopChanged();
        }
    }

    public double WindowLeft
    {
        get
        {
            string rawValue = configuration[WindowLeftKey];
            return rawValue != null ? double.Parse(rawValue) : double.NaN;
        }
        set
        {
            configuration[WindowLeftKey] = value.ToString();
            Save();
        }
    }

    public double WindowTop
    {
        get
        {
            string rawValue = configuration[WindowTopKey];
            return rawValue != null ? double.Parse(rawValue) : double.NaN;
        }
        set
        {
            configuration[WindowTopKey] = value.ToString();
            Save();
        }
    }

    public void SetWindowLocation(double left, double top)
    {
        configuration[WindowLeftKey] = left.ToString();
        configuration[WindowTopKey] = top.ToString();
        Save();
    }

    public event EventHandler KeepOnTopChanged;

    private void OnKeepOnTopChanged()
    {
        KeepOnTopChanged?.Invoke(this, EventArgs.Empty);
    }

    private void Save()
    {
        string filePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");

        string inputJson = File.ReadAllText(filePath);

        AppSettings appSettingsRoot = JsonSerializer.Deserialize<AppSettings>(inputJson);
        appSettingsRoot.KeepOnTop = KeepOnTop;
        appSettingsRoot.WindowLeft = double.IsNaN(WindowLeft)
            ? null
            : WindowLeft;
        appSettingsRoot.WindowTop = double.IsNaN(WindowTop)
            ? null
            : WindowTop;

        try
        {
            string outputJson = JsonSerializer.Serialize(appSettingsRoot, serializerOptions.Value);
            File.WriteAllText(filePath, outputJson);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
}
