using PhotoSync.Cli.Models;

namespace PhotoSync.Cli.Services.Impl;

public class SynchronisationService : ISynchronisationService
{
    #region Methods

    public IEnumerable<Photo> GetFilesToSynchronise(string inputDirectory, DateTime from)
    {
        return new DirectoryInfo(inputDirectory)
            .GetFiles("*", SearchOption.AllDirectories)
            .Where(f => f.CreationTime >= from)
            .Select(f => new Photo(f.FullName, f.Name, f.CreationTime));
    }

    public Task<SyncResult> ExecuteAsync(
        Photo file, string destination)
    {
        var isDirectoryCreated = false;
        var directoryName = "";
        var isFileCreated = false;


        var dest = Path.Combine(
            destination,
            file.CreationTime.ToString("yyyy-MM-dd")
        );

        if (!Directory.Exists(dest))
        {
            var directoryInfo = Directory.CreateDirectory(dest);

            isDirectoryCreated = true;
            directoryName = directoryInfo.Name;
        }

        var destinationFile = Path.Combine(dest, file.Name);
        if (!File.Exists(destinationFile))
        {
            File.Copy(
                file.FullName,
                destinationFile
            );
            isFileCreated = true;
        }

        return Task.FromResult(
            new SyncResult(isFileCreated, isDirectoryCreated, directoryName)
        );
    }

    #endregion
}