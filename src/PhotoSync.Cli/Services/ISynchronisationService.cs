using PhotoSync.Cli.Models;
using PhotoSync.Cli.Services.Impl;

namespace PhotoSync.Cli.Services;

public interface ISynchronisationService
{
    #region Methods

    IEnumerable<Photo> GetFilesToSynchronise(string inputDirectory, DateTime from);

    Task<SyncResult> SynchroniseAsync(Photo photo, string destination);

    #endregion
}