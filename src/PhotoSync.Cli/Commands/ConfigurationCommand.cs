using System.ComponentModel;
using ConsoleAppFramework;
using PhotoSync.Cli.Models;
using PhotoSync.Cli.Services;
using PhotoSync.Cli.Utils;

namespace PhotoSync.Cli.Commands;

/// <summary>
///     Configure the application configuration.
/// </summary>
[Description("Configure the application configuration.")]
public sealed class ConfigurationCommand
{
    #region Fields

    private readonly IConfigurationService _configService;
    private readonly IOutputService _output;

    #endregion

    #region Constructors

    public ConfigurationCommand(
        IOutputService output,
        IConfigurationService configService)
    {
        _output = output;
        _configService = configService;
    }

    #endregion

    #region Methods

    private static bool HandleParameters(
        string outputPath,
        string lastSync,
        ref Configuration configuration)
    {
        bool isUpdated;
        if (!string.IsNullOrWhiteSpace(outputPath))
        {
            configuration.OutputPath = outputPath;
            configuration.OutputPath = ConfigUtils.NormalisePath(configuration.OutputPath);
            CreateDirectory(outputPath);
            isUpdated = true;
        }

        if (!string.IsNullOrWhiteSpace(lastSync))
        {
            if (!DateTime.TryParse(lastSync, out _))
            {
                configuration.LastSync = ConfigUtils.NormalizeDate();
                isUpdated = false;
            }
            else
            {
                configuration.LastSync = ConfigUtils.NormalizeDate(lastSync);
                isUpdated = true;
            }
        }
        else
        {
            configuration.LastSync = ConfigUtils.NormalizeDate();
            isUpdated = false;
        }

        return isUpdated;
    }

    private static void CreateDirectory(string outputPath)
    {
        if (Directory.Exists(outputPath)) return;
        
        Directory.CreateDirectory(outputPath);
    }

    /// <summary>
    ///     Sets the default configuration of the tool.
    /// </summary>
    /// <param name="output">
    ///     -o,Specifies the default path where files will be saved.
    /// </param>
    /// <param name="lastSync">
    ///     -s,Specifies the threshold date used to select files for synchronisation.
    /// </param>
    /// <returns>
    ///     Returns 0 if the command executes successfully;
    ///     returns a non-zero error code otherwise.
    /// </returns>
    [Command("set")]
    public int SetConfiguration(string output = "", string lastSync = "1970-01-01")
    {
        _output.AppTitle();
        var config = _configService.LoadConfiguration();

        var isUpdated = HandleParameters(
            output,
            lastSync,
            ref config
        );

        _output.RenderKeyValueList("Configuration",
        [
            Parameter.Valid("Last sync", ConfigUtils.NormalizeDate(config.LastSync)),
            Parameter.Valid("Output path", ConfigUtils.NormalisePath(config.OutputPath))
        ]);

        if (!isUpdated) return 0;

        _configService.SaveConfiguration(config);
        _output.Information(":floppy_disk: Configuration has been updated.");
        _output.EmptyLine();
        return 0;
    } 
    
    /// <summary>
    /// Lists the configuration saved in the configuration file.  If no configuration is set, displays the default values.
    /// </summary>

    /// <returns>
    ///     Returns 0 if the command executes successfully;
    ///     returns a non-zero error code otherwise.
    /// </returns>
    [Command("show")]
    public int ShowConfiguration()
    {
        _output.AppTitle();
        var config = _configService.LoadConfiguration();

        _output.RenderKeyValueList("Configuration",
        [
            Parameter.Valid("Last sync", ConfigUtils.NormalizeDate(config.LastSync)),
            Parameter.Valid("Output path", ConfigUtils.NormalisePath(config.OutputPath))
        ]);

        return 0;
    }

    #endregion
}