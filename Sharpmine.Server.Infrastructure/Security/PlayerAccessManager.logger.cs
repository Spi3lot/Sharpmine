using Microsoft.Extensions.Logging;

namespace Sharpmine.Server.Infrastructure.Security;

public partial class PlayerAccessManager
{

    [LoggerMessage(LogLevel.Error, "An error occured while loading {FileName}")]
    partial void LogErrorWhileLoadingJson(Exception ex, string fileName);

}
