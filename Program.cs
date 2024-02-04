using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Polling;
using Telegram.Bot.Requests;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.ReplyMarkups;
using IronPdf;
using InstagramAndYoutube.InstagramController;
using InstagramAndYoutube.YoutubeController.YoutubeMp3Controller;
using InstagramAndYoutube.YoutubeController;
using Exam_Bot.MusicController;

var botClient = new TelegramBotClient("6969242005:AAHI1NT-dKDvgOXYs3TxYTTcRqZ-oGYUfdY");

var adress_users = @"C:\Users\HP\Desktop\Sheenam\ContactBot\bin\Debug\net8.0\users.json";
var adress_admins = @"C:\Users\HP\Desktop\Sheenam\ContactBot\bin\Debug\net8.0\admins.json";
var adress_users_pdf = @"C:\Users\HP\Desktop\Sheenam\ContactBot\bin\Debug\net8.0\users.pdf";

Person local_person = new Person();

#region
using CancellationTokenSource cts = new();

ReceiverOptions receiverOptions = new()
{
    AllowedUpdates = Array.Empty<UpdateType>()
};


botClient.StartReceiving(
    updateHandler: HandleUpdateAsync,
    pollingErrorHandler: HandlePollingErrorAsync,
    receiverOptions: receiverOptions,
    cancellationToken: cts.Token
);


var me = await botClient.GetMeAsync();

Console.WriteLine($"Start listening for @{me.Username}");
Console.ReadLine();


cts.Cancel();
async Task HandleUpdateAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    Task handler = update.Type switch
    {
        UpdateType.Message => HandleMessageAsync(botClient, update, cancellationToken),
        UpdateType.CallbackQuery=> CallBackController.CheckButton(botClient, update, cancellationToken),
        _ => HandleUnknownUpdateType(botClient, update, cancellationToken),
    };
    try
    {
        await handler;
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Error chiqdi:{ex.Message}");
    }
}

async Task HandleMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    var handler = update.Message.Type switch
    {
        MessageType.Text => HandleTextMessageAsync(botClient, update, cancellationToken),
        MessageType.Contact => HandleContactMessageAsync(botClient, update, cancellationToken), 
        _ => HandleUnknownUpdateType(botClient, update, cancellationToken)
    };

    await handler;

}
#endregion
async Task HandleContactMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    Message message = update.Message;
    if (CheckUserById(message!.Chat.Id) == false)
    {
        await AddModelToJsonFile(new Person() { id = message.Chat.Id, Name = message.Contact!.FirstName, Phone_Number = message.Contact.PhoneNumber });
    }
    local_person = new Person() { id = message.Chat.Id, Name = message.Contact!.FirstName, Phone_Number = message.Contact.PhoneNumber };
    if (isUserOrAdmin(local_person) == "admin")
    {
        ReplyKeyboardMarkup requestReplyKeyboard = new(
            new[]
            {
                        new KeyboardButton("/send_advertisement"),
                        new KeyboardButton("/getUsers")
            });
        requestReplyKeyboard.ResizeKeyboard = true;
        requestReplyKeyboard.OneTimeKeyboard = true;
        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: "Biror buttonni bosing!",
             replyMarkup: requestReplyKeyboard
         );
    }
    else if (isUserOrAdmin(local_person) == "user")
    {
        await botClient.SendTextMessageAsync(
             chatId: message.Chat.Id,
             text: "Musiqa nomi yoki video linkini tashlang."
         );
    }
}
async Task HandleTextMessageAsync(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    Message message = update.Message;
    if (CheckUserById(message!.Chat.Id) == false)
    {
        ReplyKeyboardMarkup markup1 = new ReplyKeyboardMarkup(KeyboardButton.WithRequestContact("Contact jo'nating."));
        markup1.ResizeKeyboard = true;
        markup1.OneTimeKeyboard = true;
        await botClient.SendTextMessageAsync(
            chatId: message!.Chat.Id,
            text: "Raqamingizni qoldiring.",
            replyMarkup: markup1
        );
        return;
    }
    if (message.Text == "/send_advertisement")
    {
        string infos;
        using (StreamReader reader = new StreamReader(adress_users))
        {
            infos = reader.ReadToEnd();
        }
        List<Person> persons = JsonSerializer.Deserialize<List<Person>>(infos);
        foreach (Person human in persons)
        {
            await botClient.SendPhotoAsync(
                chatId: human.id,
                photo: InputFile.FromUri("https://github.com/TelegramBots/book/raw/master/src/docs/photo-ara.jpg"),
                caption: "<b>Ara bird</b>. <i>Source</i>: <a href=\"https://pixabay.com\">Pixabay</a>",
                parseMode: ParseMode.Html,
                cancellationToken: cancellationToken);
        }
    }


    if (message.Text == "/getUsers")
    {
        CreateAndSendPdf(adress_users,message);
    }
    if (message.Text.StartsWith("https://www.instagram.com"))
    {
        await SendInstagramClass.EssentialFunction(botClient, update, cancellationToken);
    }
    else if (message.Text.StartsWith("https://www.youtube.com") || message.Text.StartsWith("https://youtu.be"))
    {
        await SendYoutube.EssentialFunction(botClient, update, cancellationToken);

        //await SendYoutubeMp3.EssentialFunction(botClient, update, cancellationToken);
    }
    else if(!message.Text.StartsWith("https://www") && message.Text != "/getUsers" && message.Text != "/send_advertisement")
    {
        await SendMusic.EssentialFunction(botClient, update, cancellationToken);
    }

    //else
    //{
    //    await botClient.SendTextMessageAsync(
    //        chatId: update.Message.Chat.Id,
    //        text: "Noto'g'ri link",
    //        cancellationToken: cancellationToken);
    //}

}
async Task  CreateAndSendPdf(string json_path,Message message)
{
    IronPdf.License.IsValidLicense("IRONSUITE.TEMIROVELYOR7347.GMAIL.COM.32262-F133C7C424-B5OC5PC-JZQL7ASGXSU6-UZ7UUSPA5QRH-725HELUIVLPP-J4EQUTFYQX36-B4NBS3I3NRMT-Y32ZEFPZ7RC7-RKXLCZ-TKTD4KVAMZ2LUA-DEPLOYMENT.TRIAL-OSZ2JD.TRIAL.EXPIRES.03.MAR.2024");
    string text = System.IO.File.ReadAllText(adress_users);
    ChromePdfRenderer renderer = new ChromePdfRenderer();
    PdfDocument pdf = renderer.RenderHtmlAsPdf(text);
    pdf.SaveAs(adress_users_pdf);
    await using Stream stream = System.IO.File.OpenRead(adress_users_pdf);
    await botClient.SendDocumentAsync(
        chatId: message.Chat.Id,
        document: InputFile.FromStream(stream: stream, fileName: $"All_users.pdf"),
        caption: "Hoz mo'jiza boldi karl"
        );
    stream.Dispose();
}
string isUserOrAdmin(Person person)
{
    string infos;
    using (StreamReader reader = new StreamReader(adress_admins))
    {
        infos = reader.ReadToEnd();
    }
    List<Person> persons = JsonSerializer.Deserialize<List<Person>>(infos);
    foreach (Person human in persons)
    {
        if (human.id == person.id)
        {
            return "admin";
        }
    }
    return "user";
}

async Task GetUsers(long id)
{
    string infos;
    using (StreamReader reader = new StreamReader(adress_users))
    {
        infos = reader.ReadToEnd();
    }
    List<Person> persons = JsonSerializer.Deserialize<List<Person>>(infos);
    foreach (Person human in persons)
    {
        await botClient.SendTextMessageAsync(
        chatId: id,
        text: $"\tUser Id => {human.id}\n\tUser Name => {human.Name}\n\tUser number => {human.Phone_Number}\n");
    }
}

async Task AddModelToJsonFile(Person person)
{
    string infos;
    using (StreamReader reader = new StreamReader(adress_users))
    {
        infos = reader.ReadToEnd();
    }
    List<Person> persons = JsonSerializer.Deserialize<List<Person>>(infos);
    bool justBool = true;
    foreach (Person human in persons)
    {
        if (human.id == person.id)
        {
            justBool = false;
        }
    }
    if (justBool) { persons.Add(person); }
    using (StreamWriter sw = new StreamWriter(adress_users))
    {
        string newData = JsonSerializer.Serialize(persons);
        sw.Write(newData);
    }
}

bool CheckUserById(long id)
{
    string infos;
    using (StreamReader reader = new StreamReader(adress_users))
    {
        infos = reader.ReadToEnd();
    }
    List<Person> persons = JsonSerializer.Deserialize<List<Person>>(infos);
    foreach (Person human in persons)
    {
        if (human.id == id)
        {
            return true;
        }
    }
    return false;
}


Task HandleUnknownUpdateType(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
{
    throw new NotImplementedException();
}



Task HandlePollingErrorAsync(ITelegramBotClient botClient, Exception exception, CancellationToken cancellationToken)
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
class Person
{
    public long id { get; set; }
    public string? Name { get; set; }
    public string? Phone_Number { get; set; }
}