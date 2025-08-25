namespace PhotoSync.Cli.Models;

public record SyncResult(bool IsFileCreated, bool IsDirectoryCreated, string DirectoryName);