namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Events.AzureMonitorAutoscale
{
    // Model for 'Azure Event Grid adapter for Azure Monitor Autoscale'
    // See https://github.com/tomkerkhove/azure-monitor-autoscale-to-event-grid-adapter#supported-events
    public class AzureResource
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Name { get; set; }
        public string Region { get; set; }
        public string PortalLink { get; set; }
    }
}
