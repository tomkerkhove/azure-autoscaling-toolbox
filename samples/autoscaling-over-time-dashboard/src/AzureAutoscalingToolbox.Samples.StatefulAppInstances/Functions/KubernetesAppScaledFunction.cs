using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Identifiers;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Interfaces;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Extensions;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions.Foundation;
using Kubernetes.EventGrid.Bridge.Contracts.Enums;
using Kubernetes.EventGrid.Bridge.Contracts.Events.Deployments;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions
{
    public class KubernetesAppScaledFunction : AzureEventGridFunction
    {
        protected override List<string> SupportedEventTypes { get; } = new List<string>
        {
            KubernetesEventType.DeploymentScaleIn.GetDescription(),
            KubernetesEventType.DeploymentScaleOut.GetDescription()
        };
        
        [FunctionName("kubernetes-app-scaled-event")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", "options", Route = "v1/autoscale/kubernetes/app")] HttpRequest request,
            [DurableClient] IDurableEntityClient durableEntityClient)
        {
            if (request.Method.Equals("OPTIONS", StringComparison.InvariantCultureIgnoreCase))
            {
                return PerformEventGridEndpointValidation(request);
            }

            var scalingInformation = await ParseAndGetEventPayload<DeploymentScaleEventPayload>(request);

            // We are not interested if it's not a Kubernetes Event Grid Bridge event, return HTTP 200 OK
            // See https://docs.k8s-event-grid-bridge.io/supported-events/deployments/
            if (scalingInformation == null)
            {
                return Ok();
            }

            if (scalingInformation.Replicas?.New == null)
            {
                return BadRequest("No new instance count was specified");
            }
            if (string.IsNullOrWhiteSpace(scalingInformation.Deployment?.Name))
            {
                return BadRequest("No deployment name was specified");
            }

            // Notify scaling occurred for application
            var entityId = new KubernetesEntityIdentifier(scalingInformation.Deployment.Name, scalingInformation.Deployment.Namespace).GetEntityId();
            await durableEntityClient.SignalEntityAsync<IKubernetesApplicationEntity>(entityId, proxy => proxy.Scale(scalingInformation.Replicas.New));

            return Ok();
        }
    }
}
