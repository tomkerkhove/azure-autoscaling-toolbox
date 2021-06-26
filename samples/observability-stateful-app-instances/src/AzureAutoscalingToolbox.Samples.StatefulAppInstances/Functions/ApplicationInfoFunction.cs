using System.Threading.Tasks;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions
{
    public class ApplicationInfoFunction : HttpFunction
    {
        [FunctionName("application-info-instance-count")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "v1/app/instances")] HttpRequest request,
            [DurableClient] IDurableEntityClient durableEntityClient)
        {
            // Determine the application id
            var applicationId = request.Query["appId"];
            if (string.IsNullOrWhiteSpace(applicationId))
            {
                return BadRequest("No application id was specified");
            }

            // Get the current state of our application entity
            var entityId = new EntityId(ApplicationEntity.EntityName, applicationId);
            var appIdentity = await durableEntityClient.ReadEntityStateAsync<ApplicationEntity>(entityId);

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
