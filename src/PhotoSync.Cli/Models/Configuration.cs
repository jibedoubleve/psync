namespace PhotoSync.Cli.Models;

public class Configuration
{
    #region Fields

    private const string MinDate = "1970-01-01T00:00:00.0000000";

    #endregion

    #region Properties

    private static string DefaultOutputPath =>
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
            "Photos_Temp"
        );

    public string LastSync { get; set; } = MinDate;
    public string OutputPath { get; set; } = DefaultOutputPath;

    #endregion
}