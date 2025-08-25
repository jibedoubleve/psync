using PhotoSync.Cli.Services;
using Spectre.Console;
using Spectre.Console.Cli;
using Spectre.Console.Rendering;

namespace PhotoSync.Cli.Commands;

public sealed class ListDriveCommand : Command<ListDriveCommand.Settings>
{
    #region Fields

    private readonly IDriveService _driveService;
    private readonly IOutputService _output;

    #endregion

    #region Constructors

    public ListDriveCommand(IDriveService driveService, IOutputService output)
    {
        _driveService = driveService;
        _output = output;
    }

    #endregion

    #region Methods

    public override int Execute(CommandContext context, Settings settings)
    {
       _output.AppTitle();
        var drives = _driveService.GetDcimDrives().ToArray();
        if (drives.Length == 0)
        {
            _output.Warning(":HollowRedCircle: No drives found...");
            return 0;
        }
        
        _output.RenderDriveList(drives);

        return 0;
    }

    #endregion

    public sealed class Settings : CommandSettings;
}