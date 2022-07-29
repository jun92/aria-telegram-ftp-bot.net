using Microsoft.Extensions.Hosting;
using AriaTelegramFtpBot.Services;


namespace AriaTelegramFtpBot.HostedServices;
public class TelegramCommService : IHostedService
{
    private readonly TelegramHandlerService _telegram;
    private readonly Aria2HandlerService _aria2;

    public TelegramCommService(TelegramHandlerService telegramCommService, Aria2HandlerService aria2)
    {
        _telegram = telegramCommService;
        _aria2 = aria2;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _telegram.Initialize(cancellationToken);
        await _aria2.AddQueue("test");
        Console.WriteLine("Starting..\n");
        while( !cancellationToken.IsCancellationRequested )
        {
            await _aria2.GetStatus();
            await Task.Delay(1000, cancellationToken);
        }
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        await _aria2.Shutdown();
    }
}