using System.Collections.Generic;
using System.Threading.Tasks;
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
    public class KubernetesApplicationEntity : IApplicationDurableEntity
    {
        public const string EntityName = "kubernetes-application-entity";
        private const string AppScalingInEventName = "App Scaling In";
        private const string AppScalingOutEventName = "App Scaling Out";

        private readonly ILogger<KubernetesApplicationEntity> _logger;

        [JsonProperty("instanceCount")]
        public int InstanceCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        // This constructor is used for all signal operations
        public KubernetesApplicationEntity(EntityId entityId, ILogger<KubernetesApplicationEntity> logger)
        {
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;

            Name = entityId.EntityKey;
        }

        // This constructor is used to read state
        public KubernetesApplicationEntity()
        {
            _logger = NullLogger<KubernetesApplicationEntity>.Instance;
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
                {"AppName", Name}
            };
            return contextInformation;
        }

        [FunctionName(EntityName)]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx) => ctx.DispatchAsync<KubernetesApplicationEntity>(ctx.EntityId);
    }
}
