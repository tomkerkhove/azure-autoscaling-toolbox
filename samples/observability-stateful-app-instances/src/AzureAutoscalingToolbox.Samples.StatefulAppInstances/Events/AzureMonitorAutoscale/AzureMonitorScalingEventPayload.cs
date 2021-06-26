using System.Collections.Generic;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Events.AzureMonitorAutoscale
{
    // Model for 'Azure Event Grid adapter for Azure Monitor Autoscale'
    // See https://github.com/tomkerkhove/azure-monitor-autoscale-to-event-grid-adapter#supported-events
    public class AzureMonitorScalingEventPayload
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public Dictionary<string, string> Metadata { get; set; }
        public InstanceInfo Capacity { get; set; }
        public ScaleTargetInfo ScaleTarget { get; set; }
    }
}
