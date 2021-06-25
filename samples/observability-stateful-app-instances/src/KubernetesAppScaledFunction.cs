using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace FunctionApp1
{
    public class KubernetesAppScaledFunction
    {
        private readonly ILogger<KubernetesAppScaledFunction> _logger;
        public KubernetesAppScaledFunction(ILogger<KubernetesAppScaledFunction> logger)
        {
            _logger = logger;
        }
        [FunctionName("Function1_HttpStart")]
        public async Task<IActionResult> HttpStart(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestMessage req,
            [DurableClient] IDurableEntityClient starter,
            ILogger log)
        {
            var rawRequest = await req.Content.ReadAsStringAsync();
            var instanceInfo = JsonConvert.DeserializeObject<InstanceInfo>(rawRequest);

            var entityId = new EntityId(EntityNames.App.ToString(), "deployment-1");
            
            await starter.SignalEntityAsync(entityId, OperationNames.AppScaled.ToString(), instanceInfo.InstanceCount);

            _logger.LogInformation("Entity updated");

            return new OkResult();
        }

        [FunctionName("App")]
        public void App([EntityTrigger] IDurableEntityContext ctx)
        {
            switch (ctx.OperationName.ToLowerInvariant())
            {
                case "appscaled":
                    var newInstanceCount = ctx.GetInput<int>();
                    ctx.SetState(newInstanceCount);

                    var contextualInformation = new Dictionary<string, object>
                    {
                        {"AppName", ctx.EntityId.EntityKey},
                    };
                    ILoggerExtensions.LogMetric(_logger, "Instance count", newInstanceCount, contextualInformation);
                    break;
            }
        }
    }

    public class InstanceInfo { public int InstanceCount { get; set; } }
}