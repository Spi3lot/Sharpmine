using System.Buffers;

using Optional;

using Sharpmine.Domain.Extensions;

namespace Sharpmine.Server.Infrastructure.Protocol.Packets.Play.Clientbound;

public partial record PlayerInfoUpdatePacket(PlayerActions Actions, PlayerInfoEntry[] Entries)
{

    public void SerializeContent(IBufferWriter<byte> writer)
    {
        writer.WriteByte((byte) Actions);
        writer.WritePrefixedSpan(Entries, Actions, static (writer, entry, actions) =>
        {
            writer.WriteUuid(entry.Uuid);

            if (actions.HasFlag(PlayerActions.AddPlayer))
            {
                writer.WriteString(entry.Name!);
                writer.WritePrefixedSpan(entry.Properties, static (writer, property) =>
                {
                    writer.WriteString(property.Name);
                    writer.WriteString(property.Value);
                    writer.WritePrefixedOptional(property.Signature, static (writer, value) => writer.WriteString(value));
                });
            }

            if (actions.HasFlag(PlayerActions.InitializeChat)) writer.WriteBoolean(false); // TODO: WritePrefixedOptional
            if (actions.HasFlag(PlayerActions.UpdateGameMode)) writer.WriteVarInt((int) entry.GameMode!.Value);
            if (actions.HasFlag(PlayerActions.UpdateListed)) writer.WriteBoolean(entry.Listed!.Value);
            if (actions.HasFlag(PlayerActions.UpdateLatency)) writer.WriteVarInt(entry.Ping!.Value);
            if (actions.HasFlag(PlayerActions.UpdateDisplayName)) writer.WritePrefixedOptional(entry.DisplayName.SomeNotNull(), static (writer, name) => writer.WriteJsonString(name));
            if (actions.HasFlag(PlayerActions.UpdateListPriority)) writer.WriteVarInt(entry.ListPriority!.Value);
            if (actions.HasFlag(PlayerActions.UpdateHat)) writer.WriteBoolean(entry.HatVisible!.Value);
        });
    }

}
