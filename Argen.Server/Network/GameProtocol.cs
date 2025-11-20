using SuperSocket.WebSocket;

namespace Argen.Server.Network;

public sealed class GameProtocol
{
    public async ValueTask HandlerAsync(GameSession session, WebSocketPackage package)
    {
        await session.SendAsync("Hello from server!");
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
