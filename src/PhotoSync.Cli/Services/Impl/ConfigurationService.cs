using System.Text.Json;
using PhotoSync.Cli.Models;
using PhotoSync.Cli.Utils;

namespace PhotoSync.Cli.Services.Impl;

public class ConfigurationService : IConfigurationService
{
    #region Properties
    public string ConfigurationPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "psync",
            "config.json"
        );

    #endregion

    #region Methods

    public Configuration LoadConfiguration()
    {
        if (!File.Exists(ConfigurationPath))
            return new Configuration();

        var json = File.ReadAllText(ConfigurationPath);
        return JsonSerializer.Deserialize(json, AppJsonContext.Default.Configuration) 
               ?? new Configuration();
    }
    
    public void SaveConfiguration(Configuration configuration)
    {
        configuration.OutputPath = ConfigUtils.NormalisePath(configuration.OutputPath);
        var json = JsonSerializer.Serialize(configuration, AppJsonContext.Default.Configuration);
        var directory = Path.GetDirectoryName(ConfigurationPath) ?? "";

        if (directory == string.Empty)
            throw new DirectoryNotFoundException("Path configured for configuration is wrong.");

        Directory.CreateDirectory(directory);
        File.WriteAllText(ConfigurationPath, json);
    }

    #endregion
}