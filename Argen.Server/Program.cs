using Argen.Server.Game;
using Argen.Server.Network;
using Argen.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket.Server;
using SuperSocket.Server.Abstractions.Host;
using SuperSocket.WebSocket;
using SuperSocket.WebSocket.Server;

namespace Argen.Server;

public static class Extensions
{
    public static ISuperSocketHostBuilder<WebSocketPackage> UseGameProtocol(this ISuperSocketHostBuilder<WebSocketPackage> builder)
    {
        return builder.ConfigureServices((context, services) =>
        {
            services.AddTransient<GameProtocol>();
            services.AddTransient<SessionHandlers>(sp =>
            {
                var protocol = sp.GetRequiredService<GameProtocol>();
                return new SessionHandlers
                {
                    Closed = (session, args) => protocol.OnSessionClosed((GameSession)session, args),
                    Connected = session => protocol.OnSessionConnected((GameSession)session)
                };
            });

            services.AddTransient<Func<WebSocketSession, WebSocketPackage, ValueTask>>(sp =>
            {
                return (session, package) => sp.GetRequiredService<GameProtocol>().HandlerAsync((GameSession)session, package);
            });
        });
    }
}

static class Program
{ 
    private static void Configure(HostBuilderContext context, IServiceCollection services)
    {
        services.AddSingleton<Dispatcher>();
        services.AddSingleton<World>();
        services.AddHostedService<GameLoopService>();
    }
    
    private static async Task Main(string[] args)
    {
        var host = WebSocketHostBuilder.Create()
            .UseSession<GameSession>()
            .UseGameProtocol()
            .ConfigureServices(Configure)
            .Build();
        
        await host.RunAsync();
    }
}