using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AriaTelegramFtpBot.HostedServices;

IHost MainHost = Host.CreateDefaultBuilder(args)
    .ConfigureServices( (_, services) => {
        services.AddHostedService<TelegramCommService>();

    })
    .ConfigureAppConfiguration( builder => {

    })
    .Build();
await MainHost.RunAsync();