using ConsoleAppFramework;
using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Cli.Commands;
using PhotoSync.Cli.Services;
using PhotoSync.Cli.Services.Impl;


var app = ConsoleApp.Create();

/*
 * Dependency Injection
 */
app.ConfigureServices(registrations =>
{
    registrations.AddSingleton<IOutputService, OutputService>();
    registrations.AddSingleton<IConfigurationService, ConfigurationService>();

    registrations.AddSingleton<ISynchronisationService, SynchronisationService>();
    registrations.AddSingleton<IDriveService, DriveService>();

    registrations.AddSingleton<ConfigurationCommand>();
    registrations.AddSingleton<ListDriveCommand>();
    registrations.AddSingleton<SynchronisationService>();
});

/*
 * Command Line arguments
 */
app.Add<ListDriveCommand>("list-drive");
app.Add<ConfigurationCommand>("config");
app.Add<SynchronisationCommand>("sync");

/*
 * Application start
 */
app.Run(args);