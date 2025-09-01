using System.Text.RegularExpressions;
using PhotoSync.Cli.Models;
using PhotoSync.Cli.Utils;
using Spectre.Console;
using Spectre.Console.Rendering;

namespace PhotoSync.Cli.Services.Impl;

public partial class OutputService : IOutputService
{
    #region Methods

    private void Output(string colour, bool newline, string message)
    {
        const string template = "[{0}]{1}[/]";
        var alternative = RemoveEmojiRegEx().Replace(message, string.Empty);

        var msg = AnsiConsole.Profile.Capabilities.Unicode ? message : alternative;

        msg = string.Format(template, colour, msg);

        if (newline) AnsiConsole.MarkupLine(msg);
        else AnsiConsole.Markup(msg);
    }

    [GeneratedRegex(":[ a-z_-]+:")]
    private static partial Regex RemoveEmojiRegEx();

    public void AppTitle()
    {
        const string title = "Photo Sync";
        var rule = new Rule($"[cyan]{title}[/]")
        {
            Justification = Justify.Left
        };
        AnsiConsole.Write(rule);
        EmptyLine();
    }

    public bool AskConfirmation(string question)
    {
        return AnsiConsole.Prompt(
            new TextPrompt<bool>(question)
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n"));
    }

    public void EmptyLine(int lines = 1)
    {
        for (var i = 0; i < lines; i++)
            AnsiConsole.WriteLine();
    }

    public void Information(string message, bool newline = true)
    {
        Output("white", newline, message);
    }

    public void RenderDriveList(IEnumerable<Drive> drives)
    {
        var table = new Table
        {
            Border = TableBorder.Minimal,
            ShowRowSeparators = false,
            Title = new TableTitle("[cyan][b]List all removable DCIM drives[/][/]")
        };

        table.AddColumn("[blue][b]Name[/][/]");
        table.AddColumn("[blue][b]Volume Label[/][/]");
        table.AddColumn("[blue][b]Format[/][/]");


        foreach (var di in drives) table.AddRow(di.Name, di.VolumeLabel, di.DriveFormat);

        AnsiConsole.Write(table);
    }

    public void RenderKeyValueList(string title, IEnumerable<Parameter> tableRows)
    {
        var rows = tableRows.ToArray();
        if (rows.Length < 2) return;

        var table = new Table
        {
            Border = TableBorder.Minimal,
            ShowRowSeparators = false,
            ShowHeaders = true,
            Title = new TableTitle($"[cyan][b]{title}[/][/]")
        };

        table.AddColumn(new TableColumn("[blue][b]Key[/][/]").RightAligned());
        table.AddColumn(new TableColumn("[blue][b]Value[/][/]").LeftAligned());

        foreach (var row in rows)
        {
            IRenderable key = new Markup(row.Key);

            var value = CliRenderer.IsRenderablePath(row.Value)
                ? CliRenderer.ToRenderablePath(row.Value)
                : new Markup(row.Value);

            table.AddRow(key, value);
        }

        AnsiConsole.Write(table);
    }

    public void Warning(string message)
    {
        Output("yellow", true, message);
    }

    #endregion
}