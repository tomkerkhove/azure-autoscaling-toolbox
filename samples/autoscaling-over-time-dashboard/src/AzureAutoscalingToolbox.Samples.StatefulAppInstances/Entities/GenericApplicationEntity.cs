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
    public class GenericApplicationEntity : ApplicationEntity, IGenericApplicationEntity
    {
        public const string EntityName = "application-entity";

        private readonly ILogger<GenericApplicationEntity> _logger;

        [JsonProperty("instanceCount")]
        public int InstanceCount { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        // This constructor is used for all signal operations
        public GenericApplicationEntity(EntityId entityId, ILogger<GenericApplicationEntity> logger)
            : base(logger)
        {
            Guard.NotNull(logger, nameof(logger));

            _logger = logger;

            Name = entityId.EntityKey;
        }

        // This constructor is used to read state
        public GenericApplicationEntity()
            : base(NullLogger<GenericApplicationEntity>.Instance)
        {
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

        /// <summary>
        ///     Report the current instance count as a metric
        /// </summary>
        public void ReportCurrentInstanceCount()
        {
            ReportCurrentInstanceCount(InstanceCount);
        }

        protected virtual Dictionary<string, object> GetContextInformation()
        {
            var contextInformation = base.GetContextInformation();
            contextInformation.TryAdd("AppName", Name);

            return contextInformation;
        }

        [FunctionName(EntityName)]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx) => ctx.DispatchAsync<GenericApplicationEntity>(ctx.EntityId);
    }
}
