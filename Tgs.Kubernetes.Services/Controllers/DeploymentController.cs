using k8s;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Tgs.Kubernetes.Services.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DeploymentController : ControllerBase
    {
        private readonly k8s.Kubernetes _kubernetesClient;
        private readonly KubernetesClientConfiguration _kubernetesConfig;
        private readonly ILogger<DeploymentController> _logger;

        public DeploymentController(
            k8s.Kubernetes kubernetesClient,
            KubernetesClientConfiguration kubernetesConfig,
            ILogger<DeploymentController> logger)
        {
            _kubernetesClient = kubernetesClient ?? throw new ArgumentNullException(nameof(kubernetesClient));
            _kubernetesConfig = kubernetesConfig ?? throw new ArgumentNullException(nameof(kubernetesConfig));
            _logger = logger;
        }

        [HttpGet(Name = "GetDeployments")]
        public async Task<k8s.Models.V1ComponentStatusList> Get()
        {

            var data = await _kubernetesClient.ListComponentStatusAsync(allowWatchBookmarks: true)
                .ConfigureAwait(false);

            return data;
        }
    }
}