using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace FW.Bridge.Configuration;

public static class Config
{
    private static readonly string saveFilePath = Path.Combine("./", "app.config.json");
    private static readonly string versionFilePath = Path.Combine("./", "app.config.version");
    public static ConfigData Data { get; private set; } = new();
    
    public static Version CurrentVersion => typeof(Config).Assembly.GetName().Version!;
    
    public static async Task SaveAsync()
    {
        string saveData = JsonSerializer.Serialize(Data, new JsonSerializerOptions { WriteIndented = true });
        await File.WriteAllTextAsync(saveFilePath, saveData);
        await File.WriteAllTextAsync(versionFilePath, CurrentVersion.ToString());
    }

    public static async Task LoadAsync()
    {
        await CheckFiles();
        
        Version version = Version.Parse(await File.ReadAllTextAsync(versionFilePath));
        await Upgrade(version);
        
        string loadData = await File.ReadAllTextAsync(saveFilePath);
        Data = JsonSerializer.Deserialize<ConfigData>(loadData) ?? new();
    }

    private static async Task Upgrade(Version fileVersion)
    {
        // Nothing to upgrade
        if (fileVersion >= CurrentVersion) return;
        
        // Do upgrade stuff

        await SaveAsync();
    }

    private static async Task CheckFiles()
    {
        if (!File.Exists(saveFilePath) || !File.Exists(versionFilePath))
        {
            Data = new();
            await SaveAsync();
        }
    }
}