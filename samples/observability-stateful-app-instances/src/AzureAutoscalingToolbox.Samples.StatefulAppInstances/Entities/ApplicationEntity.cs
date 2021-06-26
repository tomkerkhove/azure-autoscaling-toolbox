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
    public class ApplicationEntity : IApplicationDurableEntity
    {
        public const string EntityName = "application-entity";
        private readonly ILogger<ApplicationEntity> _logger;

        [JsonProperty("instanceCount")]
        public int InstanceCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        // This constructor is used for all signal operations
        public ApplicationEntity(EntityId entityId, ILogger<ApplicationEntity> logger)
        {
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;

            Name = entityId.EntityKey;
        }

        // This constructor is used to read state
        public ApplicationEntity()
        {
            _logger = NullLogger<ApplicationEntity>.Instance;
        }

        /// <summary>
        ///     Our application is scaling
        /// </summary>
        /// <param name="newInstanceCount">New instance count of the application</param>
        public void Scale(int newInstanceCount)
        {
            InstanceCount = newInstanceCount;

            ReportCurrentInstanceCount();
        }

        /// <summary>
        ///     Report the current instance count as a metric
        /// </summary>
        public void ReportCurrentInstanceCount()
        {
            var contextInformation = new Dictionary<string, object>
            {
                {"AppName", Name}
            };
            _logger.LogMetric("App Instances", InstanceCount, contextInformation);
        }

        [FunctionName(EntityName)]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx) => ctx.DispatchAsync<ApplicationEntity>(ctx.EntityId);
    }
}
