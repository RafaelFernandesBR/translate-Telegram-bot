using Data.Conect;
using Serilog;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TranslateBot.Commands;
public class MeuCommand : ICommandBot
{
    public string Nome => "/meu";

    public async void Executar(ITelegramBotClient botClient, ILogger logger, long chatId, Update? update = null)
    {
        var databaseconect = new DatabaseConect(logger);
        var dados = await databaseconect.Verificar(Convert.ToString(chatId));

        string msgSend;
        if (dados.Count() == 0)
        {
            msgSend = "Você ainda não definiu seu idioma de origem e destino, use o comando /start para defini-los.\nOu defina manualmente com o comando /trocar e os idiomas que quiser.";
        }
        else
        {
            msgSend = $"O seu idioma de origem salvo atualmente é {dados.First().idioma_selecionado_origem}\nE o de destino é {dados.First().idioma_selecionado_destino}";
        }

        await botClient.SendTextMessageAsync(chatId, msgSend);
    }

}
