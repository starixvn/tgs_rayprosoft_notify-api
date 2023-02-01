using Microsoft.Extensions.Configuration;
using System.Net;

namespace Tgs.Notification
{
    internal interface ITelegramApiClient
    {
        Task<bool> SendMessageAsync(string msg);
    }
    internal class TelegramApiClient : ITelegramApiClient
    {
        readonly string _telegramAPI;
        readonly string _botToken;
        protected string _channel;

        public TelegramApiClient(IConfiguration configuration)
        {
            _telegramAPI = configuration.GetSection("Telegram:API").Value;
            _botToken = configuration.GetSection("Telegram:BotToken").Value;
            _channel = configuration.GetSection("Telegram:production-PrometheusChannel").Value; 
        }


        /// <summary>
        /// Send a message to a Telegram chat/channel
        /// </summary>
        /// <param name="msg">Message text</param>
        /// <param name="sendTo">Recepient</param>
        async Task<bool> ITelegramApiClient.SendMessageAsync(string msg)
        {
            try
            {
                var message = WebUtility.UrlEncode(msg);
                using var httpClient = new HttpClient();
                var url = $"{_telegramAPI}/bot{_botToken}/sendMessage?chat_id={_channel}&text={message}&parse_mode=HTML&disable_web_page_preview=true";
                var res = await httpClient.GetAsync(url);

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Couldn't send a message via Telegram. Response from Telegram API: {await res.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
