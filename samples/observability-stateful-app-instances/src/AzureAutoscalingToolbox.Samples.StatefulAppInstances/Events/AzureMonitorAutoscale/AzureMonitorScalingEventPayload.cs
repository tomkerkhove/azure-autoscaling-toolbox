using System.Collections.Generic;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Events.AzureMonitorAutoscale
{
    public class AzureMonitorScalingEventPayload
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public InstanceInfo Capacity { get; set; }
        public ScaleTargetInfo ScaleTarget { get; set; }
    }
}
