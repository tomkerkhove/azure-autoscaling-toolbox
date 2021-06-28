namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Events.AzureMonitorAutoscale
{
    // Model for 'Azure Event Grid adapter for Azure Monitor Autoscale'
    // See https://github.com/tomkerkhove/azure-monitor-autoscale-to-event-grid-adapter#supported-events
    public class InstanceInfo
    {
        public int New { get; set; }
        public int Old { get; set; }
    }
}
