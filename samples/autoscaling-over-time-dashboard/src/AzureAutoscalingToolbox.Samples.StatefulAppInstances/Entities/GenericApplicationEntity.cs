using System.Collections.Generic;
using System.Threading.Tasks;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Identifiers;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Interfaces;
using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Models;
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
        public const string EntityName = "generic-application-entity";
        private const string AppScalingInEventName = "App Scaling In";
        private const string AppScalingOutEventName = "App Scaling Out";

        [JsonProperty("subscriptionName")]
        public string SubscriptionId { get; set; }

        [JsonProperty("resourceGroupName")]
        public string ResourceGroupName { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("instanceCount")]
        public int InstanceCount { get; set; }

        [JsonProperty("resourceName")]
        public string ResourceName { get; set; }

        [JsonProperty("runtime")]
        public string Runtime { get; set; }

        // This constructor is used for all signal operations
        public GenericApplicationEntity(EntityId entityId, ILogger<GenericEntityIdentifier> logger)
            : base(logger)
        {   
            var entityIdentifier = GenericEntityIdentifier.ParseFromString(entityId.EntityKey);
            
            Runtime = entityIdentifier.Runtime;
            ResourceName = entityIdentifier.ResourceName;
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
            ReportCurrentInstanceCountAsMetric(InstanceCount);
        }

        protected override Dictionary<string, object> GetContextInformation()
        {
            var contextInformation = base.GetContextInformation();
            contextInformation.TryAdd("SubscriptionId", SubscriptionId);
            contextInformation.TryAdd("ResourceGroup", ResourceGroupName);
            contextInformation.TryAdd("AppName", ResourceName);
            contextInformation.TryAdd("Region", Region);
            contextInformation.TryAdd("Runtime", Runtime);

            return contextInformation;
        }

        [FunctionName(EntityName)]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx) => ctx.DispatchAsync<GenericApplicationEntity>(ctx.EntityId);

        public Task StoreMetadataAsync(GenericAppInfo genericAppInfo)
        {
            Guard.NotNull(genericAppInfo, nameof(genericAppInfo));
            Guard.NotNullOrWhitespace(genericAppInfo.SubscriptionId, nameof(genericAppInfo.SubscriptionId));
            Guard.NotNullOrWhitespace(genericAppInfo.ResourceGroupName, nameof(genericAppInfo.ResourceGroupName));
            Guard.NotNullOrWhitespace(genericAppInfo.Region, nameof(genericAppInfo.Region));

            SubscriptionId = genericAppInfo.SubscriptionId;
            ResourceGroupName = genericAppInfo.ResourceGroupName;
            Region = genericAppInfo.Region;

            return Task.CompletedTask;
        }
    }
}
