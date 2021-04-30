using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TGS.Core.Infrastructures.Json;
using TGS.NotifyApi.Clients;
using TGS.NotifyApi.Messages;

namespace TGS.NotifyApi.Controllers 
{
    [ApiController]
    [Route("telegram")]
    public class TelegramController : ControllerBase
    {
        private readonly ILogger<TelegramController> _logger;
        private readonly ITelegramApiClient _client;
        private readonly IConfiguration _configuration;
        public TelegramController(ITelegramApiClient client,
            IConfiguration configuration,
            ILogger<TelegramController> logger)
        {
            _logger = logger;
            _client = client;
            _configuration = configuration;
        }

        [HttpPost("{env}/prometheus-alert")]
        public async Task<NotifyResponse> SendAsync(
            string env,
            [FromServices] IConfiguration configuration,
            [FromServices] IJsonSerializationProvider json,
            [FromBody] PrometheusAlertRequest request)
        {
            try
            {
                var data = json.SerializeToString(request);
                Console.WriteLine(data);

                var channel = _configuration.GetSection($"Telegram:{env}-PrometheusChannel").Value;

                var messageList = request.Alerts.Select(p =>
                    $@"

Alert Name: <b>{p.Labels.Alertname}</b>
Instance: {p.Labels.Instance}
Severity: {p.Labels.Severity}
Desc: <u>{p.Annotations.Description}</u>
Summary: {p.Annotations.Summary}")
                    .ToArray();

                var message = string.Join($@"", messageList);
                await _client.SendMessageAsync(message, channel);

                return new NotifyResponse
                { 
                    IsSuccess=true,
                    Message=string.Empty
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.InnerException.Message);
                throw;
            }
        }

        [HttpPost("{env}/log-alert")]
        public async Task<NotifyResponse> SendAsync(
            string env,
            [FromServices] IConfiguration configuration,
            [FromServices] IJsonSerializationProvider json,
            [FromBody] NotifyLogRequest request)
        {
            try
            {
                var channel = _configuration.GetSection($"Telegram:{env}-LogChannel").Value;
                var message = $@"
Type: [{request.Type}]
Id: {request.Id}
Prefix: {request.Prefix}
Path: <b>{request.Path}</b>
Method: <u>{request.Method}</u>
Server Name: {request.ServerName}
Response Code: {request.ResponseCode}
Recorded On: {request.RecordedOn}
Exception: {request.Exception}
Stack Trace: {request.Stacktrace}
";

                await _client.SendMessageAsync(message, channel);

                return new NotifyResponse
                {
                    IsSuccess = true,
                    Message = string.Empty
                };
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                _logger.LogError(e.InnerException.Message);
                throw;
            }
        }
    }
}
