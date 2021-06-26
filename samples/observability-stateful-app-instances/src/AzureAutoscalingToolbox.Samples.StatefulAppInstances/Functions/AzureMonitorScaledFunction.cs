using System;
using System.Linq;
using System.Threading.Tasks;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions
{
    public class AzureMonitorScaledFunction
    {
        [FunctionName("azure-monitor-scaled-app-event")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "post", Route = "v1/autoscale/azure-monitor/app")] HttpRequest request,
            [DurableClient] IDurableEntityClient durableEntityClient)
        {
            if (request.Method.Equals("OPTIONS", StringComparison.InvariantCultureIgnoreCase))
            {
                return PerformEventGridEndpointValidation(request);
            }

            // TODO: Parse CloudEvent

            // Notify scaling occurred for application
            var entityId = new EntityId(ApplicationEntity.EntityName, "app-1");
            await durableEntityClient.SignalEntityAsync<IApplicationDurableEntity>(entityId, proxy => proxy.Scale(100));

            return new OkResult();
        }


        private IActionResult PerformEventGridEndpointValidation(HttpRequest httpRequest)
        {
            // Kudos to @dbarkol for sharing this (https://gist.github.com/dbarkol/724c3302e3029af1c59183148c8cc1de#file-cloudevents-options-functions-cs)
            // Retrieve the request origin
            if (!httpRequest.Headers.TryGetValue("WebHook-Request-Origin", out var headerValues))
            {
                return new BadRequestObjectResult("Not a valid request");
            }

            // Respond with the origin and rate
            var webhookRequestOrigin = headerValues.FirstOrDefault();
            httpRequest.HttpContext.Response.Headers.Add("WebHook-Allowed-Rate", "*");
            httpRequest.HttpContext.Response.Headers.Add("WebHook-Allowed-Origin", webhookRequestOrigin);

            return new OkResult();
        }
    }
}
