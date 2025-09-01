using ConsoleAppFramework;
using PhotoSync.Cli.Models;
using PhotoSync.Cli.Services;
using PhotoSync.Cli.Utils;
using Spectre.Console;

namespace PhotoSync.Cli.Commands;

public class SynchronisationCommand
{
    #region Fields

    private readonly IConfigurationService _configurationService;
    private readonly IDriveService _driveService;
    private readonly IOutputService _output;
    private readonly ISynchronisationService _sync;
    private const string DateTemplate = "dd MMM yyyy HH:mm";

    #endregion

    #region Constructors

    public SynchronisationCommand(
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

    private Parameter HandleInputPath(string inputPath)
    {
        if (!string.IsNullOrWhiteSpace(inputPath))
            return Parameter.Invalid("Input path", "No path specified".ToError());

        inputPath = _driveService.GetFirstDcim();

        return Directory.Exists(inputPath)
            ? Parameter.Valid("Input path", inputPath)
            : Parameter.Invalid("Input path", "Invalid path!".ToError());
    }

    private static Parameter HandleLastSynchronisationDate(string lastSync, Configuration config)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(lastSync))
            {
                var date = DateTime.Parse(config.LastSync);
                return Parameter.Valid(Params.LastSync,
                    date.ToString(DateTemplate)
                );
            }

            var overridenDate = DateTime.Parse(config.LastSync).ToString(DateTemplate);
            return Parameter.Valid(Params.LastSync, overridenDate);
        }
        catch (Exception e)
        {
            return Parameter.Valid(Params.LastSync,
                e.Message.ToError()
            );
        }
    }

    private static Parameter HandleOutputPath(string outputPath, Configuration config)
    {
        if (string.IsNullOrWhiteSpace(outputPath))
            return Parameter.Invalid(Params.Output, config.OutputPath);

        outputPath = Environment.ExpandEnvironmentVariables(outputPath);
        return PathUtils.IsValidPath(outputPath)
            ? Parameter.Valid(Params.Output, outputPath)
            : Parameter.Invalid(Params.Output, "Invalid path!".ToError());
    }

    private IEnumerable<Parameter> ValidateParameters(
        Configuration config, string lastSync, string inputPath, string outputPath)
    {
        yield return HandleInputPath(inputPath);
        yield return HandleLastSynchronisationDate(lastSync, config);
        yield return HandleOutputPath(outputPath, config);
    }

    /// <summary>
    ///     Synchronises all files from input-path that were created after last-sync into output-path.
    /// </summary>
    /// <param name="input">
    ///     -i,The source directory to check for files that should be copied.
    /// </param>
    /// <param name="output">
    ///     -o,The destination directory where files created after last-sync will be copied.
    /// </param>
    /// <param name="syncDate">
    ///     -s,The threshold date; all files created after this date will be copied to output-path.
    /// </param>
    /// <param name="yes">
    ///     -y,If set, confirmation will be requested before copying files;
    ///     otherwise, files will be copied immediately.
    /// </param>
    /// <returns>
    ///     Returns 0 if the command executes successfully;
    ///     returns a non-zero error code otherwise.
    /// </returns>
    [Command("")]
    public async Task<int> ExecuteAsync(
        string input = "",
        string output = "",
        string syncDate = "1970-01-01",
        bool yes = false)
    {
        _output.AppTitle();
        var config = _configurationService.LoadConfiguration();

        var validation = ValidateParameters(config,
            syncDate,
            input,
            output).ToArray();


        _output.RenderKeyValueList("Configuration", validation);

        if (validation.Any(p => !p.IsValid))
        {
            _output.Warning("Argument validation failed.");
            return -1;
        }

        var files = _sync.GetFilesToSynchronise(
            input!,
            DateTime.Parse(syncDate!)
        );

        _output.EmptyLine();
        _output.Information($":framed_picture:  [cyan]{files.Count()}[/] file(s) to synchronise.");

        if (!yes)
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
                var result = await _sync.ExecuteAsync(file, output!);
                if (result.IsDirectoryCreated)
                    _output.Information($":file_folder: Created directory '{result.DirectoryName}'");

                progress.Increment(1);
            }
        });

        return 0;
    }

    #endregion

    private static class Params
    {
        #region Fields

        public const string LastSync = "Last synchronisation";
        public const string Output = "Output path";

        #endregion
    }
}