using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace TGS.NotifyApi.Clients
{
    public interface ITelegramApiClient
    {
        Task<bool> SendMessageAsync(string msg, string group);
    }
    public class TelegramApiClient : ITelegramApiClient
    {
        IConfiguration _configuration;
        ILogger<TelegramApiClient> _logger;

        readonly string _telegramAPI;
        readonly string _botToken;
        protected string _channel;

        public TelegramApiClient(IConfiguration configuration, ILogger<TelegramApiClient> logger)
        {
            _configuration = configuration;
            _logger = logger;

            _telegramAPI = _configuration.GetSection("Telegram:API").Value;
            _botToken = _configuration.GetSection("Telegram:BotToken").Value;

        }


        /// <summary>
        /// Send a message to a Telegram chat/channel
        /// </summary>
        /// <param name="msg">Message text</param>
        /// <param name="sendTo">Recepient</param>
        async Task<bool> ITelegramApiClient.SendMessageAsync(string msg, string channel)
        {
            try
            {
                var message = WebUtility.UrlEncode(msg);
                using var httpClient = new HttpClient();
                var url = $"{_telegramAPI}/bot{_botToken}/sendMessage?chat_id={channel}&text={message}&parse_mode=HTML&disable_web_page_preview=true";
                var res = await httpClient.GetAsync(url);

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    throw new Exception($"Couldn't send a message via Telegram. Response from Telegram API: {await res.Content.ReadAsStringAsync()}");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
            return true;
        }
    }
}
