using GuardNet;
using Microsoft.Azure.WebJobs.Extensions.DurableTask;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Identifiers
{
    public class KubernetesEntityIdentifier
    {
        public string DeploymentName { get; set; }

        public string Namespace { get; set; }

        public KubernetesEntityIdentifier(string deploymentName, string namespaceName)
        {
            Guard.NotNullOrWhitespace(deploymentName, nameof(deploymentName));
            Guard.NotNullOrWhitespace(namespaceName, nameof(namespaceName));

            DeploymentName = deploymentName;
            Namespace = namespaceName;
        }

        public static KubernetesEntityIdentifier ParseFromString(string rawEntityIdentifier)
        {
            var splitEntityKey = rawEntityIdentifier.Split("|");
            var namespaceName = splitEntityKey[0];
            var deploymentName = splitEntityKey[1];

            return new KubernetesEntityIdentifier(deploymentName, namespaceName);
        }

        public EntityId GetEntityId()
        {
            return new EntityId(KubernetesApplicationEntity.EntityName, $"{Namespace}|{DeploymentName}");
        }
    }
}
