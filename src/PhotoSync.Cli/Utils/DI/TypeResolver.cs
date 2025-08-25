using Spectre.Console.Cli;

namespace PhotoSync.Cli.Infrastructure.DI;

public sealed class TypeResolver : ITypeResolver, IDisposable
{
    #region Fields

    private readonly IServiceProvider _provider;

    #endregion

    #region Constructors

    public TypeResolver(IServiceProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    #endregion

    #region Methods

    public void Dispose()
    {
        if (_provider is IDisposable disposable) disposable.Dispose();
    }

    public object? Resolve(Type? type) => type == null ? null : _provider.GetService(type);

    #endregion
}