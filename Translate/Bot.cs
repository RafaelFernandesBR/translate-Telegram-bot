using Serilog;
using Telegram.Bot;
using Telegram.Bot.Polling;

namespace Control;
public class Bot
{
    private readonly TelegramBotClient _botClient;
    private readonly ReceiverOptions _receiverOptions;
    private readonly MessageHandler _messageHandler;
    private readonly ILogger _logger;
    private readonly ErrorHandler _ErrorHandler;

    public Bot(TelegramBotClient telegramBotClient, ILogger logger)
    {
        _logger = logger;
        _botClient = telegramBotClient;
        _receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { }
        };

        _messageHandler = new MessageHandler(logger);
        _ErrorHandler = new ErrorHandler(logger);
    }

    public async Task Start()
    {
        _botClient.StartReceiving(
            _messageHandler.HandleUpdateAsync,
            _ErrorHandler.HandleErrorAsync,
            _receiverOptions);

        var me = await _botClient.GetMeAsync();
        _logger.Debug($"Start listening for @{me.Username}");
    }

}
