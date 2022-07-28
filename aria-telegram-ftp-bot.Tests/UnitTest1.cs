using System;
using Xunit;
using AriaTelegramFtpBot.HostedServices;
using AriaTelegramFtpBot.Repositories;

namespace AriaTelegramFtpBot.Tests;

public class UnitTest1
{
    [Fact]
    public void BasicTelegramTest()
    {
        TelegramRepository tt = new TelegramRepository();
        tt.GetUpdates().GetAwaiter().GetResult();
    
        //Equals(tt.GetMe().GetAwaiter().GetResult(), "RichardTorrent001");


    }
}