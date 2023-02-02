using Microsoft.Extensions.Options;
using k8s;
using Tgs.Kubernetes.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services
    .AddTransient<HttpClientLoggingHandler>()
    .ConfigureK8sConfigSetting(
        sp =>
        {
            var config = sp.GetRequiredService<IConfiguration>();

            var isInCluster = config.GetValue<bool>("KubernetesInCluster");
            var configPath = Path.Combine(Environment.CurrentDirectory, @"kubeconfig");
            return isInCluster
                ? KubernetesClientConfiguration.InClusterConfig()
                : KubernetesClientConfiguration.BuildConfigFromConfigFile(configPath);
        },
        sp => new[] { sp.GetRequiredService<HttpClientLoggingHandler>() });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
