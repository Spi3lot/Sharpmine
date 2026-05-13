using Serilog.Events;

namespace Sharpmine.Server.Wpf;

public record LogEntry(LogEventLevel Level, string FormattedMessage);
