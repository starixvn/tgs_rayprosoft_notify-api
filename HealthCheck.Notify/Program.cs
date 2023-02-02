// See https://aka.ms/new-console-template for more information
using System.Net.Http.Json;
using Tgs.Notification.HealthCheckModels;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Tgs.Notification;

string url = "https://game-healthcheck.srvptt.com/healthchecks-api";
var httpClient = new HttpClient();
var data = await httpClient.GetFromJsonAsync<Category[]>(url)
    .ConfigureAwait(false);

var unhealthyServices = data.SelectMany(p => p.Entries)
    .Where(p=>p.Status == "Healthy")
    .Select(p=>p.Name)
    .ToArray();
var message = string.Join("\n", unhealthyServices);

using IHost host = Host.CreateDefaultBuilder(args).Build();
IConfiguration config = host.Services.GetRequiredService<IConfiguration>();
ITelegramApiClient telergramApiClient = new TelegramApiClient(config);

var msg = @$"The below services fails
{message}
-- tgs devops";

_ = await telergramApiClient.SendMessageAsync(msg).ConfigureAwait(false);


