using System.Threading.Tasks;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Identifiers;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions.Foundation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions
{
    public class ApplicationInfoFunction : HttpFunction
    {
        [FunctionName("generic-application-info")]
        public async Task<IActionResult> GetInfoForGenericApp([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/apps/generic/instances")] HttpRequest request,
            [DurableClient] IDurableEntityClient durableEntityClient)
        {
            // Determine the application name
            var applicationName = request.Query["appName"];
            if (string.IsNullOrWhiteSpace(applicationName))
            {
                return BadRequest("No application name was specified");
            }

            // Determine the runtime
            var runtime = request.Query["runtime"];
            if (string.IsNullOrWhiteSpace(runtime))
            {
                return BadRequest("No runtime was specified");
            }

            // Get the current state of our application entity
            var entityId = new GenericEntityIdentifier(runtime, applicationName).GetEntityId();
            var appIdentity = await durableEntityClient.ReadEntityStateAsync<GenericApplicationEntity>(entityId);

            // If it doesn't exist, return HTTP 404
            if (appIdentity.EntityExists == false)
            {
                return NotFound();
            }

            // If it does exist, return HTTP 200 with instance count
            return OkWithPayload(new { appIdentity.EntityState.InstanceCount });
        }

        [FunctionName("kubernetes-application-info")]
        public async Task<IActionResult> GetInfoForKubernetesApp([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/apps/kubernetes/instances")] HttpRequest request,
            [DurableClient] IDurableEntityClient durableEntityClient)
        {
            // Determine the deployment name
            var deploymentName = request.Query["deploymentName"];
            if (string.IsNullOrWhiteSpace(deploymentName))
            {
                return BadRequest("No deployment name was specified");
            }

            // Determine the namespace name
            var namespaceName = request.Query["namespaceName"];
            if (string.IsNullOrWhiteSpace(namespaceName))
            {
                return BadRequest("No namespace name was specified");
            }

            // Get the current state of our application entity
            var entityId = new KubernetesEntityIdentifier(deploymentName, namespaceName).GetEntityId();
            var appIdentity = await durableEntityClient.ReadEntityStateAsync<KubernetesApplicationEntity>(entityId);

            // If it doesn't exist, return HTTP 404
            if (appIdentity.EntityExists == false)
            {
                return NotFound();
            }

            // If it does exist, return HTTP 200 with instance count
            return OkWithPayload(new {appIdentity.EntityState.InstanceCount});
        }
    }
}
