using Serilog.Events;

namespace Sharpmine.Server.Gui;

public record LogEntry(LogEventLevel Level, string FormattedMessage);
