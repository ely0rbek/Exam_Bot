using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading.Tasks;
using System.Net.Http.Headers;

namespace Exam_Bot.MusicController
{
    public class MusicDownloader
    {
        public static async Task<string> RunApi(string link)
        {
            string newUrl = "https://shazam.p.rapidapi.com/search?term=";
            link += "&locale=en-US&offset=0&limit=10";
            newUrl += link;
            var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(newUrl),
                Headers =
    {
        { "X-RapidAPI-Key", "16e48dfbd2msh7371fe5e63f3af1p180fe6jsnee0363ed0461" },
        { "X-RapidAPI-Host", "shazam.p.rapidapi.com" },
    },
            };
            using (var response = await client.SendAsync(request))
            {
                response.EnsureSuccessStatusCode();
                var body = await response.Content.ReadAsStringAsync();
                return body;
            }
        }
    }
}
