namespace PhotoSync.Cli.Utils;

public static class Randomizer
{
    private const string Letters = "abcdefghijklmnopqrstuvwxyz";
    private static readonly Random Random = new(Letters.Length);
    public static char NextLetter()=> Letters[Random.Next(Letters.Length)];
}