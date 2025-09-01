namespace PhotoSync.Cli.Models;

public record Parameter(string Key, string Value, bool IsValid)
{
    #region Methods

    public static Parameter Invalid(string key, string value)
    {
        return new Parameter(key, value, false);
    }

    public static Parameter Valid(string key, string value)
    {
        return new Parameter(key, value, true);
    }

    #endregion
}