using Data.Conect;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TranslateBot.Commands;
public class AleCommand : ICommandBot
{
    public string Nome => "/ale";

    public async void Executar(ITelegramBotClient botClient, long chatId, Update? update = null)
    {
        var databaseconect = new DatabaseConect(LoggerConfig.CreateLogger());
        var dados = await databaseconect.Verificar(Convert.ToString(chatId));

        string[] idiomasDsp = { "auto", "pt", "en", "es", "ar", "az", "zh", "cs", "da", "nl", "eo", "fi", "fr", "de", "el", "he", "hi", "hu", "id", "ga", "it", "ja", "ko", "fa", "pl", "ru", "sk", "es", "tr", "uk", "vi" };
        //gerar um número aleatório de no máximo o tamanho do array
        int aleatorio = new Random().Next(0, idiomasDsp.Length);
        //Obter o idioma de origem e atualizar com o de destino
        var atualizado = databaseconect.Atualizar(chatId.ToString(), dados.First().idioma_selecionado_origem, idiomasDsp[aleatorio]);


        string msgSend = $"Idiomas atualizado aleatóriamente.\nIdioma origem como {atualizado.First().idioma_selecionado_origem} e destino como {atualizado.First().idioma_selecionado_destino}";
        await botClient.SendTextMessageAsync(chatId, msgSend);
    }

}
