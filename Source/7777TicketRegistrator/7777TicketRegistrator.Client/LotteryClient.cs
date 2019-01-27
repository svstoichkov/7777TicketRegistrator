namespace _7777TicketRegistrator.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Timers;

    using AngleSharp.Parser.Html;

    using Newtonsoft.Json.Linq;

    public class LotteryClient : IDisposable
    {
        private readonly HtmlParser parser = new HtmlParser();
        private readonly HttpClient client;

        public LotteryClient()
        {
            var handler = new HttpClientHandler();
            handler.CookieContainer = new CookieContainer();
            this.client = new HttpClient(handler, true);
        }

        public bool IsLoggedIn { get; set; }

        public async Task<bool> Login(string username, string password)
        {
            var content = new FormUrlEncodedContent(new[]
                                                    {
                                                        new KeyValuePair<string, string>("login_username", username),
                                                        new KeyValuePair<string, string>("login_password", password),
                                                        new KeyValuePair<string, string>("submit_form", "1")
                                                    });

            var response = await this.client.PostAsync("https://7777.bg/login/", content);
            var result = await response.Content.ReadAsStringAsync();
            var document = this.parser.Parse(result);
            var title = document.Title;
            if (title.ToLower().Contains("вход"))
            {
                this.IsLoggedIn = false;
                return false;
            }

            this.IsLoggedIn = true;
            return true;
        }

        public async Task<string> Check(string barcode)
        {
            if (barcode.Length != 15 && barcode.Length != 14 && barcode.Length != 13)
            {
                return "Невалиден баркод";
            }

            var response = await this.client.GetAsync($"https://7777.bg/loto_games/check_talon/?talon_id={barcode}");
            var result = await response.Content.ReadAsStringAsync();
            var document = this.parser.Parse(result);
            var winning = document.GetElementsByClassName("winnig-text").FirstOrDefault();
            if (winning != null)
            {
                return winning.TextContent;
            }

            var status = document.GetElementsByClassName("bordered").FirstOrDefault()?.GetElementsByTagName("h1").FirstOrDefault();
            if (status != null)
            {
                return status.TextContent;
            }

            return "Грешка";
        }

        public async Task<string> Register(string barcode)
        {
            var form = new FormUrlEncodedContent(new []
                                                 {
                                                     new KeyValuePair<string, string>("register_number", barcode), 
                                                     new KeyValuePair<string, string>("submit_form", "1"), 
                                                     new KeyValuePair<string, string>("ajax_request", "1"), 
                                                 });

            var response = await this.client.PostAsync("https://7777.bg/user/register/ticket/", form);
            var content = await response.Content.ReadAsStringAsync();

            var obj = JObject.Parse(content);

            var result = "Грешка";
            if (obj.TryGetValue("SUCCESS", out var success))
            {
                result = success.ToString();
            }

            if (obj.TryGetValue("ERRORS", out var error))
            {
                result = error.ToString();
            }

            result = result.Trim('[', ']', '"', '\r', '\n', ' ').Replace("<br>", "").Replace("<br />", "");
            return result.Substring(0, Math.Min(result.Length, 200));
        }

        public void Dispose()
        {
            this.client?.Dispose();
        }
    }
}