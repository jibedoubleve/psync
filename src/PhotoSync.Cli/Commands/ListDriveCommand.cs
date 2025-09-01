using ConsoleAppFramework;
using PhotoSync.Cli.Services;

namespace PhotoSync.Cli.Commands;

public sealed class ListDriveCommand
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

    /// <summary>
    /// Lists all drives that are DCIM-compliant.
    /// </summary>
    /// <returns>
    /// Returns 0 if the command executes successfully; 
    /// returns a non-zero error code otherwise.
    /// </returns>
    [Command("")]
    public int Execute()
    {
        _output.AppTitle();
        var drives = _driveService.GetDcimDrives().ToArray();
        if (drives.Length == 0)
        {
            _output.Warning(":hollow_red_circle: No drives found...");
            return 0;
        }

        _output.RenderDriveList(drives);

        return 0;
    }

    #endregion
}