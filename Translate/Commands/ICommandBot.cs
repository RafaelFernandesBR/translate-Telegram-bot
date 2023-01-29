using Telegram.Bot;

namespace TranslateBot.Commands;
public interface ICommandBot
{
    string Nome { get; }
    void Executar(ITelegramBotClient botClient, long chatId, string? msg = null);
}
