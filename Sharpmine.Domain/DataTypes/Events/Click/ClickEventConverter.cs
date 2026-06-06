using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sharpmine.Domain.DataTypes.Events.Click;

public class ClickEventConverter : JsonConverter<ClickEvent>
{

    public override ClickEvent Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var root = JsonElement.ParseValue(ref reader);
        string action = root.GetProperty("action").GetString()!;

        return action switch
        {
            "open_url" => new OpenUrlClickEvent(root.GetProperty("url").GetString()!),
            "open_file" => new OpenFileClickEvent(root.GetProperty("path").GetString()!),
            "run_command" => new RunCommandClickEvent(root.GetProperty("command").GetString()!),
            "suggest_command" => new SuggestCommandClickEvent(root.GetProperty("command").GetString()!),
            "change_page" => new ChangePageClickEvent(root.GetProperty("page").GetInt32()),
            "copy_to_clipboard" => new CopyToClipboardClickEvent(root.GetProperty("value").GetString()!),
            "show_dialog" => new ShowDialogClickEvent(root.GetProperty("dialog")),
            "custom" => new CustomClickEvent(root.GetProperty("id").GetString()!, root.TryGetProperty("payload", out var p) ? p.GetString() : null),
            _ => throw new JsonException($"Unknown click event action: {action}")
        };
    }

    public override void Write(Utf8JsonWriter writer, ClickEvent value, JsonSerializerOptions options)
    {
        writer.WriteStartObject();
        writer.WriteString("action", value.Action);
        switch (value)
        {
            case OpenUrlClickEvent u: writer.WriteString("url", u.Url); break;
            case OpenFileClickEvent f: writer.WriteString("path", f.Path); break;
            case RunCommandClickEvent r: writer.WriteString("command", r.Command); break;
            case SuggestCommandClickEvent s: writer.WriteString("command", s.Command); break;
            case ChangePageClickEvent p: writer.WriteNumber("page", p.Page); break;
            case CopyToClipboardClickEvent c: writer.WriteString("value", c.Value); break;
            case ShowDialogClickEvent d:
                writer.WritePropertyName("dialog");
                d.Dialog.WriteTo(writer);
                break;
            case CustomClickEvent c:
                writer.WriteString("id", c.Id);
                if (c.Payload is not null) writer.WriteString("payload", c.Payload);
                break;
        }

        writer.WriteEndObject();
    }

}
