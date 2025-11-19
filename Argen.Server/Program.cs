using Argen.Server.Game;
using Argen.Server.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SuperSocket.WebSocket.Server;

namespace Argen.Server;

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
            .ConfigureServices(Configure)
            .Build();
        
        await host.RunAsync();
    }
}