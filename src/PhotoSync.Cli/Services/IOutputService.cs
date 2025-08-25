using PhotoSync.Cli.Models;
using PhotoSync.Cli.Services.Impl;

namespace PhotoSync.Cli.Services;

public interface IOutputService
{
    #region Methods

    bool AskConfirmation(string question);

    void EmptyLine(int lines = 1);

    void AppTitle();

    void RenderDriveList(IEnumerable<Drive> drives);

    void RenderKeyValueList(string title, IEnumerable<(string Key, string Value)> tableRows);

    void Information(string message, bool newline = true);

    void Warning(string message);

    #endregion
}