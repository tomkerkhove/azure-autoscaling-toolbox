using System.Threading;
using System.Threading.Tasks;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Interfaces;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions
{
    public class ReportCurrentApplicationInstancesFunction
    {
        [FunctionName("report-current-app-instances")]
        public async Task Run([TimerTrigger("0 */5 * * * *")] TimerInfo timerInfo,
            [DurableClient] IDurableEntityClient durableEntityClient)
        {
            // Get all entity instances
            var entityQuery = new EntityQuery
            {
                EntityName = ApplicationEntity.EntityName
            };
            var foundEntities = await durableEntityClient.ListEntitiesAsync(entityQuery, CancellationToken.None);

            // Report status of every entity
            foreach (var entity in foundEntities.Entities)
            {
                await durableEntityClient.SignalEntityAsync<IApplicationDurableEntity>(entity.EntityId, proxy => proxy.ReportCurrentInstanceCount());
            }
        }
    }
}
