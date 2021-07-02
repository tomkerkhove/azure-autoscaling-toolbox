using AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Models;
using System.Threading.Tasks;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Entities.Interfaces
{
    public interface IGenericApplicationEntity : IApplicationEntity
    {
        Task StoreMetadataAsync(GenericAppInfo appInfo);
    }
}