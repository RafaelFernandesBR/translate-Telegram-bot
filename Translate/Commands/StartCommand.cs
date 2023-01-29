using Data.Conect;
using Telegram.Bot;

namespace TranslateBot.Commands;
public class StartCommand : ICommandBot
{
    public string Nome => "/start";

    public void Executar(ITelegramBotClient botClient, long chatId, string? msg = null)
    {
        var databaseconect = new DatabaseConect(LoggerConfig.CreateLogger());
        databaseconect.Salvar(Convert.ToString(chatId));

        string text = "Bem vindo ao bot!";
        botClient.SendTextMessageAsync(chatId, text);
    }

}
