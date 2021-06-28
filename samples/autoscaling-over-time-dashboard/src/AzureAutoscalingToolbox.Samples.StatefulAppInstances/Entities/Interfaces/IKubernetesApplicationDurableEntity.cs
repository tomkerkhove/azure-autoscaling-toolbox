namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Interfaces
{
    public interface IKubernetesApplicationDurableEntity
    {
        /// <summary>
        ///     Our application is scaling
        /// </summary>
        /// <param name="newInstanceCount">New instance count of the application</param>
        void Scale(int newInstanceCount);

        /// <summary>
        ///     Report the current instance count as a metric
        /// </summary>
        void ReportCurrentInstanceCount();
    }
}