using GuardNet;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Identifiers
{
    public class GenericEntityIdentifier
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public string Region { get; set; }
        public string Runtime { get; set; }
        public string ResourceName { get; set; }

        public GenericEntityIdentifier(string subscriptionId, string resourceGroupName, string region, string runtime, string resourceName)
        {
            Guard.NotNullOrWhitespace(subscriptionId, nameof(subscriptionId));
            Guard.NotNullOrWhitespace(resourceGroupName, nameof(resourceGroupName));
            Guard.NotNullOrWhitespace(region, nameof(region));
            Guard.NotNullOrWhitespace(runtime, nameof(runtime));
            Guard.NotNullOrWhitespace(resourceName, nameof(resourceName));

            SubscriptionId = subscriptionId;
            ResourceGroupName = resourceGroupName;
            Region = region;
            Runtime = runtime;
            ResourceName = resourceName;
        }

        public static GenericEntityIdentifier ParseFromString(string rawEntityIdentifier)
        {
            var splitEntityKey = rawEntityIdentifier.Split("|");
            var resourceGroupName = splitEntityKey[0];
            var subscriptionId = splitEntityKey[1];
            var region = splitEntityKey[2];
            var runtime = splitEntityKey[3];
            var resourceName = splitEntityKey[4];

            return new GenericEntityIdentifier(subscriptionId, resourceGroupName, region, runtime, resourceName);
        }

        public EntityId GetEntityId()
        {
            return new EntityId(GenericApplicationEntity.EntityName, $"{SubscriptionId}|{ResourceName}|{Region}|{Runtime}|{ResourceName}");
        }
    }
}
