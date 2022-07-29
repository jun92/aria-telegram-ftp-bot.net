using Microsoft.Extensions.Hosting;

namespace AriaTelegramFtpBot.HostedServices;
public class TelegramCommService : IHostedService
{
    public Task StartAsync(CancellationToken cancellationToken)
    {
        while( !cancellationToken.IsCancellationRequested )
        {

        }
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}