using GuardNet;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Identifiers
{
    public class GenericEntityIdentifier
    {
        public string Runtime { get; set; }
        public string ResourceName { get; set; }

        public GenericEntityIdentifier(string runtime, string resourceName)
        {
            Guard.NotNullOrWhitespace(runtime, nameof(runtime));
            Guard.NotNullOrWhitespace(resourceName, nameof(resourceName));

            Runtime = runtime;
            ResourceName = resourceName;
        }

        public static GenericEntityIdentifier ParseFromString(string rawEntityIdentifier)
        {
            var splitEntityKey = rawEntityIdentifier.Split("|");
            var runtime = splitEntityKey[0];
            var resourceName = splitEntityKey[1];

            return new GenericEntityIdentifier(runtime, resourceName);
        }

        public EntityId GetEntityId()
        {
            return new EntityId(GenericApplicationEntity.EntityName, $"{Runtime}|{ResourceName}");
        }
    }
}
