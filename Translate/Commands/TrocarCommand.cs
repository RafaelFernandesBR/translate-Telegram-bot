﻿using Data.Conect;
using Telegram.Bot;

namespace TranslateBot.Commands;
public class TrocarCommand : ICommandBot
{
    public string Nome => "/trocar";

    public void Executar(ITelegramBotClient botClient, long chatId, string? msg = null)
    {
        var databaseconect = new DatabaseConect(LoggerConfig.CreateLogger());
        var texto = msg.Split(" ");

        //se o tamanho do  array for igual a 3
        string msgSend = null;
        if (texto.Length == 3)
        {
            var atualizado = databaseconect.Atualizar(chatId.ToString(), texto[1], texto[2]);
            msgSend = $"Idioma origem atualizado para: {atualizado.First().idioma_selecionado_origem}\nIdioma destino atualizado para: {atualizado.First().idioma_selecionado_destino}";
        }
        else
        {
            msgSend = "Comando inválido, verifique e envie novamente.";
        }

        botClient.SendTextMessageAsync(chatId, msgSend);
    }

}