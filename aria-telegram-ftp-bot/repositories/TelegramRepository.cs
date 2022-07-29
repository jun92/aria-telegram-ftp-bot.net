using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;

namespace AriaTelegramFtpBot.Repositories;

public class TelegramRepository : ITelegramRepository
{
    private readonly TelegramBotClient _client;
    private long _lastUpdateId;
    private CancellationTokenSource _cts;

    public TelegramRepository()
    {
        _client = new TelegramBotClient("");
        var ReceiverOptions = new ReceiverOptions 
        {
            AllowedUpdates = Array.Empty<UpdateType>()  
        };
        _cts = new CancellationTokenSource();
        _client.StartReceiving(
            updateHandler: UpdateHandler,
            pollingErrorHandler: PollErrorHandler,
            receiverOptions: ReceiverOptions,
            cancellationToken: _cts.Token
        );
    }

    public async Task UpdateHandler(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Message is not { } message)
            return;
        // Only process text messages
        if (message.Text is not { } messageText)
            return;

        var chatId = message.Chat.Id;

        Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

        // Echo received message text
        Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: "You said:\n" + messageText,
            cancellationToken: cancellationToken);
    }
    public Task PollErrorHandler(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
    {
        var ErrorMessage = exception switch
        {
            ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
            _ => exception.ToString()
        };
        Console.WriteLine(ErrorMessage);
        return Task.CompletedTask;
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