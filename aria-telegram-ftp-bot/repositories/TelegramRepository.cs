using Telegram.Bot;
using Telegram.Bot.Types;

namespace AriaTelegramFtpBot.Repositories;

public class TelegramRepository : ITelegramRepository
{
    private readonly TelegramBotClient _client;
    private long _lastUpdateId;

    public TelegramRepository()
    {
        _client = new TelegramBotClient("");
    }

    public void GetNewMessage()
    {

    }
    public async Task<string> GetMe()
    {
        User user = await _client.GetMeAsync();
        return user.FirstName;
    }

    public async Task GetUpdates(int? offset = null)
    {
        int MessageId = 0;
        List<string> results = new ();
        Update[] updates = await _client.GetUpdatesAsync(0, 10, 5);
        foreach(var u in updates)
        {
            MessageId = u.Id;
            // Console.WriteLine(u.Id.ToString());
            // Console.WriteLine(u.Message?.Text);
        }
        while(true)
        {
            updates = await _client.GetUpdatesAsync(MessageId + 1, 100, 10);
            foreach(var u in updates)
            {
                MessageId = u.Id;
                Console.WriteLine(u.Message?.MessageId.ToString());
                Console.WriteLine(u.Message?.Text);
                Message msg = await _client.SendTextMessageAsync(u.Message.Chat.Id, $"Reply to {u.Message?.Text}");
                await Task.Delay(1000);
                await _client.EditMessageTextAsync(u.Message.Chat.Id, msg.MessageId, $"Reply to {u.Message?.Text} modified");
            }
        }
    }
}