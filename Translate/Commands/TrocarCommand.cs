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

        // se o tamanho do array for igual a 3
        string msgSend = null;
        if (texto.Length == 3)
        {
            bool valid = true;
            int id = 0;
            foreach (var item in texto)
            {
                id++;
                if (item.Length > 2)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                var atualizado = databaseconect.Atualizar(chatId.ToString(), texto[1], texto[2]);
                msgSend = $"Idioma origem atualizado para: {atualizado.First().idioma_selecionado_origem}\nIdioma destino atualizado para: {atualizado.First().idioma_selecionado_destino}";
            }
            else
            {
                logger.Error($"Algum item do texto tem mais de 2 caracteres. O texto é {texto[id]}");
                msgSend = "Comando inválido, verifique e envie novamente.";
            }
        }
        else
        {
            logger.Error($"Tamanho do texto é {texto.Length}, e o texto é {update.Message.Text}");
            msgSend = "Comando inválido, verifique e envie novamente.";
        }

        await botClient.SendTextMessageAsync(chatId, msgSend);
    }

}
