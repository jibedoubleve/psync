using PhotoSync.Cli.Models;
using PhotoSync.Cli.Services;
using Spectre.Console.Cli;

namespace PhotoSync.Cli.Commands;

public sealed class ConfigurationCommand : Command<ConfigurationCommand.Settings>
{
    #region Fields

    private readonly IConfigurationService _config;
    private readonly IDriveService _driveService;

    private readonly IOutputService _output;

    #endregion

    #region Constructors

    public ConfigurationCommand(
        IOutputService output,
        IConfigurationService config,
        IDriveService driveService)
    {
        _output = output;
        _config = config;
        _driveService = driveService;
    }

    #endregion

    #region Methods

    private bool Override(Settings settings, Configuration with)
    {
        var updated = false;
        if (!string.IsNullOrEmpty(settings.LastSync))
        {
            _config.ThrowIfInvalidDate(with.LastSync);
            with.LastSync = settings.LastSync;
            updated = true;
        }

        if (!string.IsNullOrEmpty(settings.OutputPath))
        {
            with.OutputPath = settings.OutputPath;
            updated = true;
        }

        return updated;
    }

    public override int Execute(CommandContext context, Settings settings)
    {
        _output.AppTitle();
        var configuration = _config.LoadConfiguration();

        var isUpdated = Override(settings, configuration);

        _output.RenderKeyValueList("Configuration",
        [
            ("Last sync", DateTime.Parse(configuration.LastSync).ToString("g")),
            ("Output path", _config.NormalisePath(configuration.OutputPath)),
            ("First DCIM drive", _driveService.GetFirstDcim())
        ]);

        if (!isUpdated && !settings.IsInitializing) return 0;

        _output.Information(":floppy_disk: Updating configuration.");
        _config.SaveConfiguration(configuration);
        _output.EmptyLine();
        return 0;
    }

    #endregion

    public class Settings : CommandSettings
    {
        #region Properties

        [CommandOption("-i|--input")] public string InputPath { get; set; } = "";
        [CommandOption("--init")] public bool IsInitializing { get; set; }
        [CommandOption("-d|--sync-date")] public string LastSync { get; set; } = "";
        [CommandOption("-o|--output")] public string OutputPath { get; set; } = "";

        #endregion
    }
}