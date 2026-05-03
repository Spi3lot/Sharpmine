using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Security;

public partial class PlayerAccessManager
{

    [LoggerMessage(LogLevel.Error, "An error occorued while loading a json file")]
    partial void LogErrorWhileLoadingJson(Exception ex);

}
