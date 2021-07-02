using System.Collections.Generic;
using System.Threading.Tasks;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Identifiers;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Interfaces;
using GuardNet;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities
{
    [JsonObject(MemberSerialization.OptIn)]
    public class AppServiceApplicationEntity : IApplicationEntity
    {
        public const string EntityName = "app-service-application-entity";
        private const string AppScalingInEventName = "App Scaling In";
        private const string AppScalingOutEventName = "App Scaling Out";

        private readonly ILogger<AppServiceApplicationEntity> _logger;

        [JsonProperty("subscriptionName")]
        public string SubscriptionId { get; set; }

        [JsonProperty("resourceGroup")]
        public string ResourceGroup { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("instanceCount")]
        public int InstanceCount { get; set; }

        [JsonProperty("planName")]
        public string PlanName { get; set; }

        // This constructor is used for all signal operations
        public AppServiceApplicationEntity(EntityId entityId, ILogger<AppServiceApplicationEntity> logger)
        {
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;

            var kubernetesEntityIdentifier = AppServiceEntityIdentifier.ParseFromString(entityId.EntityKey);
            DeploymentName = kubernetesEntityIdentifier.DeploymentName;
            Namespace = kubernetesEntityIdentifier.Namespace;
        }

        // This constructor is used to read state
        public AppServiceApplicationEntity()
        {
            _logger = NullLogger<AppServiceApplicationEntity>.Instance;
        }

        /// <summary>
        ///     Our application is scaling
        /// </summary>
        /// <param name="newInstanceCount">New instance count of the application</param>
        public void Scale(int newInstanceCount)
        {
            ReportScalingAction(InstanceCount, newInstanceCount);

            InstanceCount = newInstanceCount;

            ReportCurrentInstanceCount();
        }

        private void ReportScalingAction(int currentInstanceCount, int newInstanceCount)
        {
            var eventName = currentInstanceCount < newInstanceCount ? AppScalingOutEventName : AppScalingInEventName;
            var contextInformation = GetContextInformation();
            
            _logger.LogEvent(eventName, contextInformation);
        }

        /// <summary>
        ///     Report the current instance count as a metric
        /// </summary>
        public void ReportCurrentInstanceCount()
        {
            var contextInformation = GetContextInformation();
            _logger.LogMetric("App Instances", InstanceCount, contextInformation);
        }

        private Dictionary<string, object> GetContextInformation()
        {
            var contextInformation = new Dictionary<string, object>
            {
                {"SubscriptionId", SubscriptionId},
                {"ResourceGroup", ResourceGroup},
                {"Region", Region},
                {"Runtime", "Azure App Service"},
            };

            return contextInformation;
        }

        [FunctionName(EntityName)]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx) => ctx.DispatchAsync<AppServiceApplicationEntity>(ctx.EntityId);
    }
}
