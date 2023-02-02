using TranslateBot.Commands;
using Telegram.Bot.Types.Enums;
using Telegram.Bot;
using Serilog;
using Telegram.Bot.Types;
using Translate.Google;
using Data.Conect;

namespace Control;
public class MessageHandler
{
    private readonly IList<TranslateBot.Commands.ICommandBot> _commands;
    private readonly ILogger _logger;
    private DatabaseConect _databaseconect;
    private TranslateGoogle _translategoogle;

    public MessageHandler(ILogger logger)
    {
        _commands = new List<TranslateBot.Commands.ICommandBot>
        {
            new StartCommand(),
            new TrocarCommand(),
            new MeuCommand(),
            new TodosCommand(),
            new AleCommand()
        };

        _logger = logger;
        _databaseconect = new DatabaseConect(logger);
        _translategoogle = new TranslateGoogle();
    }

    public async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
    {
        if (update.Type != UpdateType.Message)
            return;

        if (update.Message!.Type != MessageType.Text)
            return;

        var chatId = update.Message.Chat.Id;
        var messageText = update.Message.Text;

        if (!messageText.StartsWith("/"))
        {
            // not a command, handle as normal message
            var dados = await _databaseconect.Verificar(Convert.ToString(chatId));
            var text = await _translategoogle.TranslateTextAsync(messageText, dados.First().idioma_selecionado_origem, dados.First().idioma_selecionado_destino);
            await SendMessageAsync(chatId, text, update, botClient);
            return;
        }

        var commandName = messageText.Split(" ")[0];
        var command = _commands.FirstOrDefault(x => x.Nome == commandName);
        if (command == null)
        {
            // command not found
            return;
        }

        command.Executar(botClient, chatId, update);
    }

    private async Task SendMessageAsync(long chatId, string text, Update update, ITelegramBotClient botClient)
    {
        try
        {
            await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyToMessageId: update.Message.MessageId);
        }
        catch (Exception ex)
        {
            _logger.Error(ex.Message);
        }
    }
}
