using PhotoSync.Cli.Models;
using PhotoSync.Cli.Services;
using Spectre.Console;
using Spectre.Console.Cli;

namespace PhotoSync.Cli.Commands;

public class SynchronizationCommand : AsyncCommand<SynchronizationCommand.Settings>
{
    #region Fields

    private readonly IConfigurationService _configurationService;
    private readonly IDriveService _driveService;
    private readonly IOutputService _output;
    private readonly ISynchronisationService _sync;

    #endregion

    #region Constructors

    public SynchronizationCommand(
        IOutputService output,
        IConfigurationService configurationService,
        IDriveService driveService,
        ISynchronisationService sync)
    {
        _output = output;
        _configurationService = configurationService;
        _driveService = driveService;
        _sync = sync;
    }

    #endregion

    #region Methods

    private (string Key, string Value) HandleInputPath(Settings settings)
    {
        if (settings.InputPath is not null) return ("Input path", settings.InputPath ?? "");

        var dcim = _driveService.GetFirstDcim();

        settings.InputPath = string.IsNullOrEmpty(dcim)
            ? throw new DriveNotFoundException("No DCIM drives")
            : dcim;

        if (Directory.Exists(settings.InputPath)) return ("Input path", settings.InputPath ?? "");

        _output.Warning("Input path not found");
        return ("Input path", "Invalid path");
    }

    private static (string Key, string Value) HandleLastSynchronisationDate(Settings settings, Configuration config)
    {
        if (settings.LastSync is null)
        {
            var date = DateTime.Parse(config.LastSync);

            settings.LastSync = date.ToString("o");
            return ("Last synchronisation", date.ToString("dd MMM yyyy HH:mm"));
        }

        var overridenDate = DateTime.Parse(config.LastSync).ToString("dd MMM yyyy HH:mm");
        return ("Last synchronisation", overridenDate);
    }

    private static (string Key, string Value) HandleOutputPath(Settings settings, Configuration config)
    {
        if (settings.OutputPath is null)
        {
            settings.OutputPath = config.OutputPath;
            return ("Output path", config.OutputPath);
        }

        settings.OutputPath = Environment.ExpandEnvironmentVariables(settings.OutputPath);
        return ("Output path", settings.OutputPath);
    }

    public override async Task<int> ExecuteAsync(CommandContext context, Settings settings)
    {
        _output.AppTitle();
        var config = _configurationService.LoadConfiguration();
        _output.EmptyLine();
        _output.RenderKeyValueList("Configuration",
        [
            HandleLastSynchronisationDate(settings, config),
            HandleInputPath(settings),
            HandleOutputPath(settings, config)
        ]);

        var files = _sync.GetFilesToSynchronise(
            settings.InputPath!,
            DateTime.Parse(settings.LastSync!)
        );

        _output.EmptyLine();
        _output.Information($":framed_picture:  [cyan]{files.Count()}[/] file(s) to synchronise.");

        if (!settings.IsConfirmed)
        {
            var confirmed = _output.AskConfirmation("Do you want to synchronise all files?");
            if (!confirmed) return 0;
        }

        _output.EmptyLine();
        await AnsiConsole.Progress().StartAsync(async ctx =>
        {
            var progress = ctx.AddTask("[green]Synchronising photos...[/]");
            progress.MaxValue = files.Count();

            foreach (var file in files)
            {
                var result = await _sync.SynchroniseAsync(file, settings.OutputPath!);
                if (result.IsDirectoryCreated)
                    _output.Information($":file_folder: Created directory '{result.DirectoryName}'");

                progress.Increment(1);
            }
        });

        return 0;
    }

    #endregion

    public class Settings : CommandSettings
    {
        #region Properties

        [CommandOption("-i|--input")] public string? InputPath { get; set; }
        [CommandOption("-y|--yes")] public bool IsConfirmed { get; set; }
        [CommandOption("-d|--date")] public string? LastSync { get; set; }
        [CommandOption("-o|--output")] public string? OutputPath { get; set; }

        #endregion
    }
}