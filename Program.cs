using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Newtonsoft.Json;
using translate.Translate;
using telegram.tokem;
using Data.Conect;

StreamReader r = new StreamReader("telegram.json");
string readFile = r.ReadToEnd();
TelegramTokem telegramTokem = JsonConvert.DeserializeObject<TelegramTokem>(readFile);

var botClient = new TelegramBotClient(telegramTokem.tokem);
var fin = new Translate();
var verifica = new DatabaseConect();

using var cts = new CancellationTokenSource();

// StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
var receiverOptions = new ReceiverOptions
{
    AllowedUpdates = { } // receive all update types
};
botClient.StartReceiving(
HandleUpdateAsync,
HandleErrorAsync,
receiverOptions,
cancellationToken: cts.Token);

var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");

//definindo para fechar o programa no terminal com ctrl+c
var cancellationTokenSource = new CancellationTokenSource();
AppDomain.CurrentDomain.ProcessExit += (s, e) => cancellationTokenSource.Cancel();
Console.CancelKeyPress += (s, e) => cancellationTokenSource.Cancel();
await Task.Delay(-1, cancellationTokenSource.Token).ContinueWith(t => { });

// Send cancellation request to stop bot
cts.Cancel();

async Task MsgSendAsync(long chatId, string text, Update update, CancellationToken cancellationToken)
{
    Message sentMessage = await botClient.SendTextMessageAsync(
chatId: chatId,
text: text,
replyToMessageId: update.Message.MessageId,
cancellationToken: cancellationToken);
}

async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    // Only process Message updates: https://core.telegram.org/bots/api#message
    if (update.Type != UpdateType.Message)
        return;
    // Only process text messages
    if (update.Message!.Type != MessageType.Text)
        return;

    var chatId = update.Message.Chat.Id;
    var messageText = update.Message.Text;

    if (messageText == "/start")
    {
        verifica.Salvar(Convert.ToString(chatId));

        string msgSend = "Bem vindo ao bot de tradução, apenas mande um texto que irei tentar tradusir para o português.\nCom o comando /todos você consegue visualizar todos os idiomas suportado pelo bot.\nO idioma padrão selecionado para novos usuários é origem pt e destino en.";
        await MsgSendAsync(chatId, msgSend, update, cancellationToken);
        await MsgSendAsync(chatId, "Com o comando /trocar, você muda os seus idiomas padrões\nPor exemplo, para mudar origem e destino para espanhol e inglês, ficaria assin:\n/trocar es en\nLembre-se de colocar espassos, ezatamente como está.", update, cancellationToken);
    }

    else if (messageText.Contains("/trocar", StringComparison.OrdinalIgnoreCase))
    {
        //separar a menssagem por espassos
        var texto = messageText.Split(" ");
        string[] atualizado = verifica.Atualizar(chatId.ToString(), texto[1], texto[2]);

        string msgSend = $"Idioma origem atualizado para: {atualizado[0]}\nIdioma destino atualizado para: {atualizado[1]}";
        await MsgSendAsync(chatId, msgSend, update, cancellationToken);
    }

    else if (messageText == "/todos")
    {
        var msgSend = await fin.AllLanguagesAsync();

        await MsgSendAsync(chatId, msgSend, update, cancellationToken);
    }
    else
    {
        string[] dados = verifica.Verificar(Convert.ToString(chatId));
        string msgSend = await fin.TranslateTextAsync(messageText, dados[0], dados[1]);

        await MsgSendAsync(chatId, msgSend, update, cancellationToken);
    }
}

Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
