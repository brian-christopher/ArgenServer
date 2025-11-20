using Argen.Common;
using Argen.Common.Events;
using SuperSocket.WebSocket;

namespace Argen.Server.Network;

public sealed class GameProtocol
{
    public async ValueTask HandlerAsync(GameSession session, WebSocketPackage package)
    {
        var response = new Message();

        response.Opcode = Opcode.SPAWN_CHARACTER_EVENT;
        response.SerializePayload(new CharacterSpawnedEvent
        {
            Id = 1,
            Name = "Hero",
            X = 4,
            Y = 3
        });

        await session.SendAsync(response.ToJson());
    }

    public ValueTask OnSessionClosed(GameSession session, object? args)
    {
        // Handle session opened logic here
        return ValueTask.CompletedTask;
    }

    public ValueTask OnSessionConnected(GameSession session)
    { 
        return ValueTask.CompletedTask;
    }
}
