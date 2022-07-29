using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using AriaTelegramFtpBot.HostedServices;
using AriaTelegramFtpBot.Services;
using AriaTelegramFtpBot.Repositories;

IHost MainHost = Host.CreateDefaultBuilder(args)
    .ConfigureServices( (_, services) => {
        services.AddHostedService<TelegramCommService>();
        services.AddSingleton<ITelegramRepository, TelegramRepository>();
        services.AddSingleton<TelegramHandlerService>();
        services.AddSingleton<Aria2HandlerService>();
    })    
    .Build();
await MainHost.RunAsync();