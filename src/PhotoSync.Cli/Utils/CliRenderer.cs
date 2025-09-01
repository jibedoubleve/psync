using Spectre.Console;
using Spectre.Console.Rendering;

namespace PhotoSync.Cli.Utils;

public static class CliRenderer
{
    #region Methods

    public static bool IsRenderablePath(string rowValue)
    {
        return rowValue.Contains(Path.DirectorySeparatorChar);
    }

    public static string ToError(this string message)
    {
        const string errorTemplate = ":cross_mark: [red]{0}[/]";
        return string.Format(errorTemplate, message);
    }


    public static IRenderable ToRenderablePath(string path)
    {
        return new TextPath(path)
        {
            RootStyle = new Style(Color.Red),
            SeparatorStyle = new Style(Color.Green),
            StemStyle = new Style(Color.Blue),
            LeafStyle = new Style(Color.Yellow)
        };
    }

    #endregion
}