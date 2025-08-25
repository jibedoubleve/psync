namespace PhotoSync.Cli.Models;

public record Drive
{
    #region Properties

    public string DriveFormat { get; init; } = string.Empty;
    public string Name { get; init; } = string.Empty;
    public string RootDirectory { get; init; } = string.Empty;
    public string VolumeLabel { get; init; } = string.Empty;

    #endregion
}