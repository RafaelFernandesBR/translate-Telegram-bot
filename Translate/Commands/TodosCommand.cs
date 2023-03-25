using Telegram.Bot;
using Telegram.Bot.Types;

namespace TranslateBot.Commands;
public class TodosCommand : ICommandBot
{
    public string Nome => "/todos";

    public async void Executar(ITelegramBotClient botClient, long chatId, Update? update = null)
    {
        string[] idiomasDsp = { "auto", "pt", "en", "es", "ar", "az", "zh", "cs", "da", "nl", "eo", "fi", "fr", "de", "el", "he", "hi", "hu", "id", "ga", "it", "ja", "ko", "fa", "pl", "ru", "sk", "es", "tr", "uk", "vi" };
        string msgSend = null;

        for (int i = 0; i < idiomasDsp.Length; i++)
        {
            msgSend += idiomasDsp[i] + "\n";
        }

        await botClient.SendTextMessageAsync(chatId, msgSend!);
    }

}
