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
            string infos;
            using (StreamReader reader = new StreamReader(@"C:\Users\HP\Desktop\Exam_Bot\MusicController\uri_path.json"))
            {
                infos = reader.ReadToEnd();
            }
            List<string> Musics = JsonConvert.DeserializeObject<List<string>>(infos);

            for(int i = 0;i<Musics.Count;i++)
            {
                if (Convert.ToInt64(callbackQuery.Data) == i) 
                {
                    await botClient.SendAudioAsync(
                    chatId: update.CallbackQuery.Message.Chat.Id,
                    audio: InputFile.FromUri(Musics[i]),
                    cancellationToken: cancellationToken);
                }
            }

            //await botClient.SendAudioAsync(
            //    chatId: update.CallbackQuery.Message.Chat.Id,
            //    audio: InputFile.FromUri(update.CallbackQuery.Data),
            //    cancellationToken: cancellationToken);

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
