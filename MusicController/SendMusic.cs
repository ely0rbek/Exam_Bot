using InstagramAndYoutube.InstagramController;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types;
using Telegram.Bot;
using Microsoft.Extensions.Logging;
using Telegram.Bot.Types.ReplyMarkups;

namespace Exam_Bot.MusicController
{
    public class SendMusic
    {
        public static  async Task EssentialFunction(ITelegramBotClient botClient, Update update, CancellationToken cancellationToken)
        {
            string link = update.Message.Text;
            RootMusic Musics = JsonConvert.DeserializeObject<RootMusic>(MusicDownloader.RunApi(link).Result);

            List<string> music_uris = new List<string>();
            var buttons= new List<InlineKeyboardButton>();
            int i = 1; string all_music_names = "";
            foreach (var music in Musics.tracks.hits)
            {
                all_music_names += $"{i}) {music.track.share.subject}\n";
                buttons.Add(InlineKeyboardButton.WithCallbackData(text: $"{i}", callbackData: $"{i-1}"));
                i++;
                music_uris.Add(music.track.hub.actions[1].uri);
            }
            using (StreamWriter sw = new StreamWriter(@"C:\Users\HP\Desktop\Exam_Bot\MusicController\uri_path.json"))
            {
                string newData = JsonConvert.SerializeObject(music_uris);
                sw.Write(newData);
            }


            await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: all_music_names,
                    replyMarkup: new InlineKeyboardMarkup(buttons),
                    cancellationToken: cancellationToken);

        }
    }
}
