using PhotoSync.Cli.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace PhotoSync.Cli.Services.Impl;

public class ConfigurationService : IConfigurationService
{
    #region Properties

    private static IDeserializer Deserializer => new DeserializerBuilder()
        .WithNamingConvention(PascalCaseNamingConvention.Instance)
        .Build();

    private static ISerializer Serializer =>
        new SerializerBuilder()
            .WithNamingConvention(PascalCaseNamingConvention.Instance)
            .Build();

    public string ConfigurationPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "psync",
            "config.yaml"
        );

    #endregion

    #region Methods

    public Configuration LoadConfiguration()
    {
        if (!File.Exists(ConfigurationPath))
            return new Configuration();

        var yaml = File.ReadAllText(ConfigurationPath);
        return Deserializer.Deserialize<Configuration>(yaml) ?? new Configuration();
    }

    public string NormalisePath(string path)
    {
        return Environment.ExpandEnvironmentVariables(
            path
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar));
    }

    public void SaveConfiguration(Configuration configuration)
    {
        configuration.OutputPath = NormalisePath(configuration.OutputPath);
        var yaml = Serializer.Serialize(configuration);
        var directory = Path.GetDirectoryName(ConfigurationPath) ?? "";

        if (directory == string.Empty)
            throw new DirectoryNotFoundException("Path configured for configuration is wrong.");

        Directory.CreateDirectory(directory);
        File.WriteAllText(ConfigurationPath, yaml);
    }

    public void ThrowIfInvalidDate(string isoDate)
    {
        if (DateTime.TryParse(isoDate, out _)) return;

        throw new InvalidCastException($"The date format is not correct. Cannot parse '{isoDate}'");
    }

    #endregion
}