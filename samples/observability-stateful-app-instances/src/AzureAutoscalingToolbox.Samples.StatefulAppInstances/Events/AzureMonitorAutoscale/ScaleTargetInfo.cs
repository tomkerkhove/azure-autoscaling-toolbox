namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Events.AzureMonitorAutoscale
{
    public class ScaleTargetInfo
    {
        public string SubscriptionId { get; set; }
        public string ResourceGroupName { get; set; }
        public AzureResource Resource { get; set; }
    }
}
