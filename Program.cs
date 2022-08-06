using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Data.Conect;
using translate.Balancer;
using translate.Translate;
using Conect.data;

internal class Program
{
    private static async Task Main(string[] args)
    {
        Getdata Getdata = new Getdata();
        var dadosConect = Getdata.GetdataAll();

        var botClient = new TelegramBotClient(dadosConect.telegram.tokem);
        var verifica = new DatabaseConect();
        Balancer Balancer = new Balancer();

        using var cts = new CancellationTokenSource();

        // StartReceiving does not block the caller thread. Receiving is done on the ThreadPool.
        var receiverOptions = new ReceiverOptions
        {
            AllowedUpdates = { } // receive all update types
        };
        botClient.StartReceiving(
        HandleUpdateAsync,
        HandleErrorAsync,
        receiverOptions,
        cancellationToken: cts.Token);

        var me = await botClient.GetMeAsync();

        Console.WriteLine($"Start listening for @{me.Username}");

        //definindo para fechar o programa no terminal com ctrl+c
        var cancellationTokenSource = new CancellationTokenSource();
        AppDomain.CurrentDomain.ProcessExit += (s, e) => cancellationTokenSource.Cancel();
        Console.CancelKeyPress += (s, e) => cancellationTokenSource.Cancel();
        await Task.Delay(-1, cancellationTokenSource.Token).ContinueWith(t => { });

        // Send cancellation request to stop bot
        cts.Cancel();

        async Task MsgSendAsync(long chatId, string text, Update update, CancellationToken cancellationToken)
        {
            try
            {
                Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: chatId,
            text: text,
            replyToMessageId: update.Message.MessageId,
            cancellationToken: cancellationToken);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

        }

        async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            // Only process Message updates: https://core.telegram.org/bots/api#message
            if (update.Type != UpdateType.Message)
                return;
            // Only process text messages
            if (update.Message!.Type != MessageType.Text)
                return;

            var chatId = update.Message.Chat.Id;
            var messageText = update.Message.Text;
            string Username = update.Message.Chat.Username;

            if (messageText == "/start")
            {
                verifica.Salvar(Convert.ToString(chatId));

                string msgSend = "Olá @" + Username + " Bem vindo ao bot de tradução, apenas mande um texto que irei tentar tradusir para o português.\nCom o comando /todos você consegue visualizar todos os idiomas suportado pelo bot.\nO idioma padrão selecionado para novos usuários é origem pt e destino en.";
                await MsgSendAsync(chatId, msgSend, update, cancellationToken);
                await MsgSendAsync(chatId, "Com o comando /trocar, você muda os seus idiomas padrões\nPor exemplo, para mudar origem e destino para espanhol e inglês, ficaria assin:\n/trocar es en\nLembre-se de colocar espassos, ezatamente como está.\nCom o comando /ale, você pode gerar um idioma de destino aleatóriamente para seu chat.", update, cancellationToken);
            }

            if (messageText.Contains("/admmsg", StringComparison.OrdinalIgnoreCase))
            {
                string[] dados = verifica.Verificar(Convert.ToString(chatId));
                //remover o comando da menssagem
                var texto = messageText.Split("/admmsg ");

                if (dados == null)
                {
                    string msgSend = "Você ainda não definiu seu idioma de origem e destino, ou esse comando não é permitido para seu usuário. use o comando /start para defini-los.\nOu defina manualmente com o comando /trocar e os idiomas que quiser.";
                    await MsgSendAsync(chatId, msgSend, update, cancellationToken);
                }
                else
                {
                    if (dados[2] == "True")
                    {
                        string[] AllChatId = await verifica.GetUsersAsync();
                        await MsgSendAsync(chatId, "Pronto!", update, cancellationToken);

                        for (var i = 0; i < AllChatId.Length; i++)
                        {
                            string msgSend = texto[1];
                            Console.WriteLine("Send msg for\n" + AllChatId[i]);

                            try
                            {
                                Message sentMessage = await botClient.SendTextMessageAsync(
            chatId: AllChatId[i],
            text: msgSend,
            cancellationToken: cancellationToken);
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex);
                            }

                        }
                    }
                }
            }

            else if (messageText.Contains("/trocar", StringComparison.OrdinalIgnoreCase))
            {
                //separar a menssagem por espassos
                var texto = messageText.Split(" ");
                //se o tamanho do  array for igual a 3
                string msgSend = null;
                if (texto.Length == 3)
                {
                    string[] atualizado = atualizado = verifica.Atualizar(chatId.ToString(), texto[1], texto[2]);
                    msgSend = $"Idioma origem atualizado para: {atualizado[0]}\nIdioma destino atualizado para: {atualizado[1]}";
                }
                else
                {
                    msgSend = "Comando inválido, verifique e envie novamente.";
                }

                await MsgSendAsync(chatId, msgSend, update, cancellationToken);
            }

            else if (messageText == "/ale")
            {
                string[] dados = verifica.Verificar(Convert.ToString(chatId));

                if (dados == null)
                {
                    string msgSend = "Você ainda não definiu seu idioma de origem e destino, use o comando /start para defini-los.\nOu defina manualmente com o comando /trocar e os idiomas que quiser.";
                    await MsgSendAsync(chatId, msgSend, update, cancellationToken);
                }
                else
                {
                    var Translate = new TranslateLibretranslate();

                    string[] idiomas = await Translate.RandomAsync();
                    //gerar um número aleatório de no máximo o tamanho do array
                    int aleatorio = new Random().Next(0, idiomas.Length);
                    //Obter o idioma de origem e atualizar com o de destino
                    string[] atualizado = atualizado = verifica.Atualizar(chatId.ToString(), dados[0], idiomas[aleatorio]);

                    string msgSend = $"O idioma aleatório gerado foi: {atualizado[1]}\nJá iremos atualizar o seu idioma destino.";
                    await MsgSendAsync(chatId, msgSend, update, cancellationToken);
                }
            }

            else if (messageText == "/meu")
            {
                string[] dados = verifica.Verificar(Convert.ToString(chatId));

                if (dados == null)
                {
                    string msgSend = "Você ainda não definiu seu idioma de origem e destino, use o comando /start para defini-los.\nOu defina manualmente com o comando /trocar e os idiomas que quiser.";
                    await MsgSendAsync(chatId, msgSend, update, cancellationToken);
                }
                else
                {
                    string msgSend = $"O seu idioma de origem salvo atualmente é {dados[0]}\nE o de destino é {dados[1]}";
                    await MsgSendAsync(chatId, msgSend, update, cancellationToken);
                }
            }

            else if (messageText == "/todos")
            {
                var Translate = new TranslateLibretranslate();

                var msgSend = await Translate.AllLanguagesAsync();

                await MsgSendAsync(chatId, msgSend, update, cancellationToken);
            }
            else
            {
                string[] dados = verifica.Verificar(Convert.ToString(chatId));

                //se der erro na verificação do chatId
                if (dados == null)
                {
                    string msgSend = "Você ainda não definiu seu idioma de origem e destino, use o comando /start para defini-los.\nOu defina manualmente com o comando /trocar e os idiomas que quiser.";
                    await MsgSendAsync(chatId, msgSend, update, cancellationToken);
                }
                else
                {
                    string msgSend = await Balancer.TranslateTextAsync(messageText, dados[0], dados[1]);
                    await MsgSendAsync(chatId, msgSend, update, cancellationToken);
                }
            }
        }

        Task HandleErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException
                => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            Console.WriteLine(ErrorMessage);
            return Task.CompletedTask;
        }
    }
}