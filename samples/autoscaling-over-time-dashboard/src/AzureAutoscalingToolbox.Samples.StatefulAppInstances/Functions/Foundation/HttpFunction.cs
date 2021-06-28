using Microsoft.AspNetCore.Mvc;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions.Foundation
{
    public class HttpFunction
    {
        protected BadRequestObjectResult BadRequest(string error)
        {
            return new BadRequestObjectResult(error);
        }

        protected OkResult Ok()
        {
            return new OkResult();
        }

        protected OkObjectResult OkWithPayload(object response)
        {
            return new OkObjectResult(response);
        }

        protected NotFoundResult NotFound()
        {
            return new NotFoundResult();
        }
    }
}