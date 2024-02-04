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

            var buttons= new List<InlineKeyboardButton>();
            int i = 1; string all_music_names = "";
            foreach (var music in Musics.tracks.hits)
            {
                all_music_names += $"{i}) {music.track.share.subject}\n";
                buttons.Add(InlineKeyboardButton.WithCallbackData(text: $"{i}", callbackData: music.track.hub.actions[1].uri));
                i++;
            }


            //var markup = new InlineKeyboardMarkup(
            //new InlineKeyboardButton[][]
            //{
            //    new InlineKeyboardButton[]
            //    {
            //        InlineKeyboardButton
            //            .WithCallbackData(text: "1", callbackData: $"1-{link}"),
            //        InlineKeyboardButton
            //            .WithCallbackData(text: "2", callbackData: $"2-{link}"),
            //        InlineKeyboardButton
            //            .WithCallbackData(text: "3", callbackData: $"3-{link}"),
            //        InlineKeyboardButton
            //            .WithCallbackData(text: "4", callbackData: $"4-{link}"),
            //        InlineKeyboardButton
            //            .WithCallbackData(text: "5", callbackData: $"5-{link}"),
            //    }
            //}
            //);
            await botClient.SendTextMessageAsync(
                    chatId: update.Message.Chat.Id,
                    text: all_music_names,
                    replyMarkup: new InlineKeyboardMarkup(buttons),
                    cancellationToken: cancellationToken);

        }
    }
}
