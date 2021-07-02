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
    public class KubernetesApplicationEntity : ApplicationEntity, IKubernetesApplicationEntity
    {
        public const string EntityName = "kubernetes-application-entity";

        [JsonProperty("instanceCount")]
        public int InstanceCount { get; set; }

        [JsonProperty("deploymentName")]
        public string DeploymentName { get; set; }

        [JsonProperty("namespace")]
        public string Namespace { get; set; }

        // This constructor is used for all signal operations
        public KubernetesApplicationEntity(EntityId entityId, ILogger<KubernetesApplicationEntity> logger)
            : base(logger)
        {
            var kubernetesEntityIdentifier = KubernetesEntityIdentifier.ParseFromString(entityId.EntityKey);
            DeploymentName = kubernetesEntityIdentifier.DeploymentName;
            Namespace = kubernetesEntityIdentifier.Namespace;
        }

        // This constructor is used to read state
        public KubernetesApplicationEntity()
            : base(NullLogger<KubernetesApplicationEntity>.Instance)
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

        public void ReportCurrentInstanceCount()
        {
            ReportCurrentInstanceCountAsMetric(InstanceCount);
        }

        protected override Dictionary<string, object> GetContextInformation()
        {
            var contextInformation = base.GetContextInformation();
            contextInformation.TryAdd("AppName", DeploymentName);
            contextInformation.TryAdd("Namespace", Namespace);
            contextInformation.TryAdd("Runtime", "Kubernetes");

            return contextInformation;
        }

        [FunctionName(EntityName)]
        public static Task Run([EntityTrigger] IDurableEntityContext ctx) => ctx.DispatchAsync<KubernetesApplicationEntity>(ctx.EntityId);
    }
}
