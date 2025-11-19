using System.Diagnostics;
using Argen.Server.Game;
using Microsoft.Extensions.Hosting;

namespace Argen.Server.Services;

public class GameLoopService : BackgroundService
{
    private readonly World _world;
    private readonly Dispatcher _dispatcher;

    public GameLoopService(World world, Dispatcher dispatcher)
    {
        _world = world;
        _dispatcher = dispatcher;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var tickRate = TimeSpan.FromMilliseconds(50);
        var stopwatch = Stopwatch.StartNew();
        var accumulated = TimeSpan.Zero;
        
        var timer = new PeriodicTimer(tickRate);
        
        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            accumulated += stopwatch.Elapsed;
            stopwatch.Restart();

            while (accumulated >= tickRate)
            {
                await _dispatcher.FlushPendingAsync();
                _world.Update(tickRate);
                accumulated -= tickRate;
            }
        }
    }
}