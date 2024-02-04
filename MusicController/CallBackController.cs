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
            int its = Convert.ToInt16(callbackQuery.Data);
            await botClient.SendAudioAsync(
            chatId: update.CallbackQuery.Message.Chat.Id,
            audio: InputFile.FromUri(Musics[its]),
            cancellationToken: cancellationToken);
        }
    }
}
