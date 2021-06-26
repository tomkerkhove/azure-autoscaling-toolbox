namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Events.AzureMonitorAutoscale
{
    // Model for 'Azure Event Grid adapter for Azure Monitor Autoscale'
    // See https://github.com/tomkerkhove/azure-monitor-autoscale-to-event-grid-adapter#supported-events
    public class ScaleTargetInfo
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public AzureResource Resource { get; set; }
    }
}
