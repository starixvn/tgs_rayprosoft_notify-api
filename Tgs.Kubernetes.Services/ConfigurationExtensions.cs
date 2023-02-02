using k8s;

namespace Tgs.Kubernetes.Services
{
    public static class ConfigurationExtensions
    {

        public static IServiceCollection ConfigureK8sConfigSetting(this IServiceCollection services, 
            Func<IServiceProvider, KubernetesClientConfiguration> k8sConfigFactory,
            Func<IServiceProvider, DelegatingHandler[]>? registerHttpClientHandlerFactory = null)
        {
            services
             .AddSingleton(k8sConfigFactory)
             .AddSingleton(sp => new k8s.Kubernetes(
                 sp.GetRequiredService<KubernetesClientConfiguration>(),
                 (registerHttpClientHandlerFactory ?? new Func<IServiceProvider, DelegatingHandler[]>(_ => Array.Empty<DelegatingHandler>()))(sp)));

            return services;
        }
    }
}
