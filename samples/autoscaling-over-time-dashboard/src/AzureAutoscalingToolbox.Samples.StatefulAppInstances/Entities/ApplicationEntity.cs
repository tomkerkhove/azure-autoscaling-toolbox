using GuardNet;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities
{
    public class ApplicationEntity
    {
        private const string AppScalingInEventName = "App Scaling In";
        private const string AppScalingOutEventName = "App Scaling Out";

        protected ILogger Logger { get; }

        public ApplicationEntity(ILogger logger)
        {
            Guard.NotNull(logger, nameof(Logger));

            Logger = logger;
        }

        /// <summary>
        ///     Report the current instance count as a metric
        /// </summary>
        protected virtual void ReportCurrentInstanceCountAsMetric(int instanceCount)
        {
            var contextInformation = GetContextInformation();
            Logger.LogMetric("App Instances", instanceCount, contextInformation);
        }

        protected virtual void ReportScalingAction(int currentInstanceCount, int newInstanceCount)
        {
            var eventName = currentInstanceCount < newInstanceCount ? AppScalingOutEventName : AppScalingInEventName;
            var contextInformation = GetContextInformation();

            Logger.LogEvent(eventName, contextInformation);
        }

        protected virtual Dictionary<string, object> GetContextInformation()
        {
            var contextInformation = new Dictionary<string, object>();
            return contextInformation;
        }
    }
}
