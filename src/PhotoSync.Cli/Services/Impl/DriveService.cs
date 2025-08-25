using PhotoSync.Cli.Models;

namespace PhotoSync.Cli.Services.Impl;

public class DriveService : IDriveService
{
    #region Methods

    private bool IsDcim(DriveInfo drive)
    {
        var path = Path.Combine(drive.RootDirectory.FullName, "DCIM");
        return Directory.Exists(path);
    }

    public IEnumerable<Drive> GetDcimDrives()
    {
        var drives = DriveInfo.GetDrives()
            .Where(di => di.DriveType == DriveType.Removable && di.IsReady)
            .Where(IsDcim)
            .Select(d => new Drive
            {
                
                VolumeLabel = d.VolumeLabel,
                RootDirectory = d.RootDirectory.FullName,
                DriveFormat = d.DriveFormat
            })
            .ToArray();

        return drives;
    }

    public string GetFirstDcim()
    {
        if (GetDcimDrives().FirstOrDefault() is null) return string.Empty;

        return Path.Combine(
            GetDcimDrives().FirstOrDefault()!.RootDirectory,
            "DCIM"
        );
    }

    #endregion
}