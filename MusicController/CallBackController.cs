using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types;
using Telegram.Bot;
using Newtonsoft.Json;

namespace Exam_Bot.MusicController
{
    public class CallBackController
    {
        public static async Task CheckButton(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            CallbackQuery callbackQuery = update.CallbackQuery;
            //string[] newstr = callbackQuery.Data.Split("-");
            //RootMusic Musics = JsonConvert.DeserializeObject<RootMusic>(MusicDownloader.RunApi(newstr[1]).Result);

            await botClient.SendAudioAsync(
                chatId: update.CallbackQuery.Message.Chat.Id,
                audio: InputFile.FromUri(update.CallbackQuery.Data),
                cancellationToken: cancellationToken);
            //else if (callbackQuery.Data.StartsWith("2"))
            //{
            //    string[] newstr = callbackQuery.Data.Split("-");
            //    RootMusic Musics = JsonConvert.DeserializeObject<RootMusic>(MusicDownloader.RunApi(newstr[1]).Result);

            //    await botClient.SendAudioAsync(
            //        chatId: update.CallbackQuery.Message.Chat.Id,
            //        audio: InputFile.FromUri(Musics.tracks.hits[1].track.hub.actions[1].uri),
            //        title: Musics.tracks.hits[1].track.title,
            //        performer: Musics.tracks.hits[1].track.subtitle,
            //        cancellationToken: cancellationToken);
            //}
            //else if (callbackQuery.Data.StartsWith("3"))
            //{
            //    string[] newstr = callbackQuery.Data.Split("-");
            //    RootMusic Musics = JsonConvert.DeserializeObject<RootMusic>(MusicDownloader.RunApi(newstr[1]).Result);

            //    await botClient.SendAudioAsync(
            //        chatId: update.CallbackQuery.Message.Chat.Id,
            //        audio: InputFile.FromUri(Musics.tracks.hits[2].track.hub.actions[1].uri),
            //        title: Musics.tracks.hits[2].track.title,
            //        performer: Musics.tracks.hits[2].track.subtitle,
            //        cancellationToken: cancellationToken);
            //}
            //else if (callbackQuery.Data.StartsWith("4"))
            //{
            //    string[] newstr = callbackQuery.Data.Split("-");
            //    RootMusic Musics = JsonConvert.DeserializeObject<RootMusic>(MusicDownloader.RunApi(newstr[1]).Result);

            //    await botClient.SendAudioAsync(
            //        chatId: update.CallbackQuery.Message.Chat.Id,
            //        audio: InputFile.FromUri(Musics.tracks.hits[3].track.hub.actions[1].uri),
            //        title: Musics.tracks.hits[3].track.title,
            //        performer: Musics.tracks.hits[3].track.subtitle,
            //        cancellationToken: cancellationToken);
            //}
            //else
            //{
            //    string[] newstr = callbackQuery.Data.Split("-");
            //    RootMusic Musics = JsonConvert.DeserializeObject<RootMusic>(MusicDownloader.RunApi(newstr[1]).Result);

            //    await botClient.SendAudioAsync(
            //        chatId: update.CallbackQuery.Message.Chat.Id,
            //        audio: InputFile.FromUri(Musics.tracks.hits[4].track.hub.actions[1].uri),
            //        title: Musics.tracks.hits[4].track.title,
            //        performer: Musics.tracks.hits[4].track.subtitle,
            //        cancellationToken: cancellationToken);
            //}

        }
    }
}
