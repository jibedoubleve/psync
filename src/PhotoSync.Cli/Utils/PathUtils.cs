namespace PhotoSync.Cli.Utils;

public class PathUtils
{
    #region Methods

    public static bool IsValidPath(string path)
    {
        if (string.IsNullOrWhiteSpace(path)) { return false; }

        var invalidChars = Path.GetInvalidPathChars();
        if (path.Any(c => invalidChars.Contains(c))) { return false; }

        try
        {
            _ = Path.GetFullPath(path);
            return !Path.HasExtension(path);
        }
        catch { return false; }
    }

    #endregion
}