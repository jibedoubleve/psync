using PhotoSync.Cli.Models;

namespace PhotoSync.Cli.Services;

public interface IConfigurationService
{
    #region Properties

    string ConfigurationPath { get; }

    #endregion

    #region Methods

    Configuration LoadConfiguration();

    void SaveConfiguration(Configuration configuration);

    #endregion
}