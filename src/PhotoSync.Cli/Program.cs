using Microsoft.Extensions.DependencyInjection;
using PhotoSync.Cli.Commands;
using PhotoSync.Cli.Infrastructure.DI;
using PhotoSync.Cli.Services;
using PhotoSync.Cli.Services.Impl;
using Spectre.Console.Cli;

/*
 * Dependency Injection
 */
var registrations = new ServiceCollection();
registrations.AddSingleton<IOutputService, OutputService>();
registrations.AddSingleton<IConfigurationService, ConfigurationService>();

registrations.AddSingleton<ISynchronisationService, SynchronisationService>();
registrations.AddSingleton<IDriveService, DriveService>();

var registrar = new TypeRegistrar(registrations);

/*
 * Command Line arguments
 */
var app = new CommandApp(registrar);
app.Configure(cfg =>
{
    cfg.AddCommand<ListDriveCommand>("list");
    cfg.AddCommand<ConfigurationCommand>("config");
    cfg.AddCommand<SynchronizationCommand>("sync");
});

/*
 * Application start
 */
app.Run(args);