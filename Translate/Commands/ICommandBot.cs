using Telegram.Bot;
using Telegram.Bot.Types;

namespace TranslateBot.Commands;
public interface ICommandBot
{
    string Nome { get; }
    void Executar(ITelegramBotClient botClient, long chatId, Update? update=null);
}
