﻿using Data.Conect;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Types;

namespace TranslateBot.Commands;
public class StartCommand : ICommandBot
{
    public string Nome => "/start";

    public void Executar(ITelegramBotClient botClient, long chatId, Update? update = null)
    {
        var databaseconect = new DatabaseConect(LoggerConfig.CreateLogger());
        databaseconect.Salvar(Convert.ToString(chatId));

        string msgSend = $"Olá @{update.Message.Chat.Username} Bem vindo ao bot de tradução, apenas mande um texto que irei tentar tradusir.\nCom o comando /todos você consegue visualizar todos os idiomas suportado pelo bot.\nO idioma padrão selecionado para novos usuários é origem pt e destino en.";
        botClient.SendTextMessageAsync(chatId, msgSend);
        botClient.SendTextMessageAsync(chatId, "Com o comando /trocar, você muda os seus idiomas padrões\nPor exemplo, para mudar origem e destino para espanhol e inglês, ficaria assin:\n/trocar es en\nLembre-se de colocar espassos, ezatamente como está.\nCom o comando /ale, você pode gerar um idioma de destino aleatóriamente para seu chat.");
    }

}
