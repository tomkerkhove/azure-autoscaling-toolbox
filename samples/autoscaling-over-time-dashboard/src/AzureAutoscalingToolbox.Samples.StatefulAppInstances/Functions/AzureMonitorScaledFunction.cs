using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Identifiers;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Interfaces;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Models;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Events.AzureMonitorAutoscale;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions.Foundation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions
{
    public class AzureMonitorScaledFunction : AzureEventGridFunction
    {
        protected override List<string> SupportedEventTypes { get; } = new List<string> { AzureMonitorAutoscaleScaledInEventType, AzureMonitorAutoscaleScaledOutEventType };

        private const string AzureMonitorAutoscaleScaledInEventType = "Azure.Monitor.Autoscale.ScaleIn.Activated";
        private const string AzureMonitorAutoscaleScaledOutEventType = "Azure.Monitor.Autoscale.ScaleOut.Activated";

        [FunctionName("azure-monitor-scaled-app-event")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", "options", Route = "v1/autoscale/azure-monitor/app")] HttpRequest request,
            [DurableClient] IDurableEntityClient durableEntityClient)
        {
            if (request.Method.Equals("OPTIONS", StringComparison.InvariantCultureIgnoreCase))
            {
                return PerformEventGridEndpointValidation(request);
            }

            var scalingInformation = await ParseAndGetEventPayload<AzureMonitorScalingEventPayload>(request);

            // We are not interested if it's not an Azure Monitor Autoscale event, return HTTP 200 OK
            // See https://github.com/tomkerkhove/azure-monitor-autoscale-to-event-grid-adapter#supported-events
            if (scalingInformation == null)
            {
                return Ok();
            }

            if (scalingInformation.Capacity?.New == null)
            {
                return BadRequest("No new instance count was specified");
            }

            var resource = scalingInformation?.ScaleTarget?.Resource;
            if (resource == null)
            {
                return BadRequest("No resource information was provided");
            }
            if (string.IsNullOrWhiteSpace(resource.Name))
            {
                return BadRequest("No resource name was specified");
            }

            // Notify scaling occurred for application
            var appRuntime = DetermineApplicationRuntime(resource.Type);
            var entityId = new GenericEntityIdentifier(appRuntime, resource.Name).GetEntityId();
            
            // Store the application info for enriching metrics
            var appInfo = new GenericAppInfo
            {
                SubscriptionId = scalingInformation.ScaleTarget.SubscriptionId,
                ResourceGroupName = scalingInformation.ScaleTarget.ResourceGroupName,
                Region = resource.Region
            };
            await durableEntityClient.SignalEntityAsync<IGenericApplicationEntity>(entityId, proxy => proxy.StoreMetadataAsync(appInfo));

            // Scale the entity!
            await durableEntityClient.SignalEntityAsync<IGenericApplicationEntity>(entityId, proxy => proxy.Scale(scalingInformation.Capacity.New));

            return Ok();
        }

        private string DetermineApplicationRuntime(string type)
        {
            switch(type.ToLowerInvariant())
            {
                case "microsoft.web/serverfarms":
                    return "Azure App Service";
                default:
                    return type.Replace("/", " ");
            }
        }
    }
}
