using PhotoSync.Cli.Models;
using PhotoSync.Cli.Services.Impl;

namespace PhotoSync.Cli.Services;

public interface IDriveService
{
    #region Methods

    string GetFirstDcim();
    IEnumerable<Drive> GetDcimDrives();

    #endregion
}