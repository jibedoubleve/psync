using Spectre.Console;
using Spectre.Console.Rendering;

namespace PhotoSync.Cli.Utils;

public static class PathRenderer
{
    #region Methods

    public static bool IsRenderable(string rowValue)
    {
        return rowValue.Contains(Path.DirectorySeparatorChar);
    }


    public static IRenderable ToRenderable(string path)
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