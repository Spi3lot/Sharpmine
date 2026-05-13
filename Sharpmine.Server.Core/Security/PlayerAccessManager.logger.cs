using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Core.Security;

public partial class PlayerAccessManager
{

    [LoggerMessage(LogLevel.Error, "An error occured while loading a json file")]
    partial void LogErrorWhileLoadingJson(Exception ex);

}
