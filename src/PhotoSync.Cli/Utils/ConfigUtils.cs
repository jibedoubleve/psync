namespace PhotoSync.Cli.Utils;

public static class ConfigUtils
{
    #region Properties

    private static string DefaultDate => new DateTime(1970, 1, 1).ToString("g");

    #endregion

    #region Methods

    public static string NormalisePath(string path)
    {
        return Environment.ExpandEnvironmentVariables(
            path
                .Replace('/', Path.DirectorySeparatorChar)
                .Replace('\\', Path.DirectorySeparatorChar));
    }

    public static string NormalizeDate(string? date = null)
    {
        if (string.IsNullOrWhiteSpace(date)) return DefaultDate;

        return DateTime.TryParse(date, out var dateTime)
            ? dateTime.ToString("g")
            : DefaultDate;
    }

    #endregion
}