using System.Text.Json.Serialization;
using PhotoSync.Cli.Models;

namespace PhotoSync.Cli.Utils;

[JsonSerializable(typeof(Configuration))]
public partial class AppJsonContext : JsonSerializerContext
{
}