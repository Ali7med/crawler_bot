 
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Extensions.Polling;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;

using HtmlAgilityPack; 
using TelegramBolt.classes;

var _websiteClass =new WebsiteClass();
var botClient = new TelegramBotClient("Telegram-bot-Token");

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
Console.ReadLine();

// Send cancellation request to stop bot
cts.Cancel();

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
    Message sentMessage1; 
    try
    {
        if(messageText == "/")
        {
            sentMessage1 = await botClient.SendTextMessageAsync(chatId: chatId,text: "/karabla for example chose karbala city in site en.sat24.com/en/ like :  https://en.sat24.com/en/forecast/h/2635157/karbala", cancellationToken: cancellationToken);
            sentMessage1 = await botClient.SendTextMessageAsync(chatId: chatId,text: "/baghdad", cancellationToken: cancellationToken);

        }
        else if (messageText != "")
        {

            if(messageText == "/karabla")
            {
                messageText = "https://en.sat24.com/en/forecast/h/2635157/karbala";
            }
            else if(messageText == "/baghdad")
            {
                messageText = "https://en.sat24.com/en/forecast/h/2638451/baghdad";
            }
            else
            {
            }
                var result= _websiteClass.getDataWether(messageText);
                if(result.Count() > 0)
                {
                    foreach(var item in result)
                    {
                        sentMessage1 = await botClient.SendTextMessageAsync(chatId: chatId, text: "timecell :\n" + item.timecell, cancellationToken: cancellationToken);
                        sentMessage1 = await botClient.SendTextMessageAsync(chatId: chatId, text: "icon :\n" + item.iconPath, cancellationToken: cancellationToken);
                        sentMessage1 = await botClient.SendTextMessageAsync(chatId: chatId, text: "temperature :\n" + item.temperature, cancellationToken: cancellationToken);
                        sentMessage1 = await botClient.SendTextMessageAsync(chatId: chatId, text: "wind :\n" + item.precipitation, cancellationToken: cancellationToken);
                        sentMessage1 = await botClient.SendTextMessageAsync(chatId: chatId, text: "pressure :\n" + item.pressure, cancellationToken: cancellationToken);
                    }
                }
                else
                {
                    sentMessage1 = await botClient.SendTextMessageAsync(chatId: chatId, text: "your location invalid  , chose another page ", cancellationToken: cancellationToken);
                }


        }
        else
        {
            Console.WriteLine($"Received a '{messageText}' message in chat {chatId}.");

            // Echo received message text
            Message sentMessage = await botClient.SendTextMessageAsync(
                chatId: chatId,
                text: "You said:\n" + messageText, cancellationToken: cancellationToken);
        }
        
           
    }
    catch (Exception ex)
    {

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

