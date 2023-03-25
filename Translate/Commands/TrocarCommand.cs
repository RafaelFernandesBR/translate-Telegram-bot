using Data.Conect;
using Serilog;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TranslateBot.Commands;
public class TrocarCommand : ICommandBot
{
    public string Nome => "/trocar";

    public async void Executar(ITelegramBotClient botClient, ILogger logger, long chatId, Update? update = null)
    {
        var databaseconect = new DatabaseConect(logger);
        var texto = update.Message.Text.Split(" ");

        //se o tamanho do  array for igual a 3
        string msgSend = null;
        if (texto.Length == 3)
        {
            var atualizado = databaseconect.Atualizar(chatId.ToString(), texto[1], texto[2]);
            msgSend = $"Idioma origem atualizado para: {atualizado.First().idioma_selecionado_origem}\nIdioma destino atualizado para: {atualizado.First().idioma_selecionado_destino}";
        }
        else
        {
            logger.Error($"Tamanho do texto é {texto.Length}, e o texto é {update.Message.Text}");
            msgSend = "Comando inválido, verifique e envie novamente.";
        }

        await botClient.SendTextMessageAsync(chatId, msgSend);
    }

}
