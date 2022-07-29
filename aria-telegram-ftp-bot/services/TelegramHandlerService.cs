using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Polling;
using Telegram.Bot.Exceptions;

namespace AriaTelegramFtpBot.Services;
public class TelegramHandlerService
{
    private readonly TelegramBotClient _client;
    private CancellationTokenSource _cts;
    public TelegramHandlerService()
    {
        _client = new TelegramBotClient(Environment.GetEnvironmentVariable("TELEGRAM_API_KEY"));
        
    }
    public void Initialize(CancellationToken token)
    {
        var ReceiverOptions = new ReceiverOptions 
        {
            AllowedUpdates = Array.Empty<UpdateType>()  
        };
        _client.StartReceiving(
            updateHandler: UpdateHandler,
            pollingErrorHandler: PollErrorHandler,
            receiverOptions: ReceiverOptions,
            cancellationToken: token
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
}