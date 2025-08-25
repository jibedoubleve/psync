using PhotoSync.Cli.Models;

namespace PhotoSync.Cli.Services;

public interface IConfigurationService
{
    #region Properties

    string ConfigurationPath { get; }

    #endregion

    #region Methods

    Configuration LoadConfiguration();
    string NormalisePath(string path);

    void SaveConfiguration(Configuration configuration);

    void ThrowIfInvalidDate(string isoDate);

    #endregion
}