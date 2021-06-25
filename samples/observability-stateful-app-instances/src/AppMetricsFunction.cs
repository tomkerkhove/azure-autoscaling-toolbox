using System.Collections.Generic;
using System.Net.Http;
using System.Runtime.InteropServices.ComTypes;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace FunctionApp1
{
    public class Function1
    {
        private readonly ILogger<Function1> _logger;
        public Function1(ILogger<Function1> logger)
        {
            _logger = logger;
        }
        [FunctionName("report-app-instances")]
        public async Task ReportMetric([TimerTrigger("0 */5 * * * *")] TimerInfo timer,
            [DurableClient] IDurableEntityClient durableEntityClient)
        {
            var query = new EntityQuery()
            {
                EntityName = EntityNames.App.ToString()
            };
            var entities = await durableEntityClient.ListEntitiesAsync(query, CancellationToken.None);
            foreach (var entity in entities.Entities)
            {
                _logger.LogInformation($"Entity {entity.EntityId} found");
                //var appInfo = await durableEntityClient.ReadEntityStateAsync<AppInfo>(entity.EntityId);
                var readResult = await durableEntityClient.ReadEntityStateAsync<int>(entity.EntityId);
                
                var contextualInformation = new Dictionary<string, object>
                {
                    {"AppName", entity.EntityId.EntityKey},
                };
                ILoggerExtensions.LogMetric(_logger, "Instance count", readResult.EntityState, contextualInformation);
            }
        }
    }

    public class AppInfo
    {

    }

    public enum EntityNames
    {
        App
    }

    public enum OperationNames
    {
        AppScaled
    }
}