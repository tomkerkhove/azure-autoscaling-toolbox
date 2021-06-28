using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloudNative.CloudEvents;
using CloudNative.CloudEvents.AspNetCore;
using CloudNative.CloudEvents.NewtonsoftJson;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

namespace AzureAutoscalingToolbox.Samples.StatefulAppInstances.Functions.Foundation
{
    public abstract class AzureEventGridFunction : HttpFunction
    {
        private readonly JsonEventFormatter _jsonEventFormatter = new JsonEventFormatter();

        protected abstract List<string> SupportedEventTypes { get; }

        protected IActionResult PerformEventGridEndpointValidation(HttpRequest httpRequest)
        {
            // Kudos to @dbarkol for sharing this (https://gist.github.com/dbarkol/724c3302e3029af1c59183148c8cc1de#file-cloudevents-options-functions-cs)
            // Retrieve the request origin
            if (!httpRequest.Headers.TryGetValue("WebHook-Request-Origin", out var headerValues))
            {
                return BadRequest("Not a valid request");
            }

            // Respond with the origin and rate
            var webhookRequestOrigin = headerValues.FirstOrDefault();
            httpRequest.HttpContext.Response.Headers.Add("WebHook-Allowed-Rate", "*");
            httpRequest.HttpContext.Response.Headers.Add("WebHook-Allowed-Origin", webhookRequestOrigin);

            return new OkResult();
        }

        protected async Task<TPayload> ParseAndGetEventPayload<TPayload>(HttpRequest request)
            where TPayload : class
        {
            // Parse request to CloudEvent
            CloudEvent cloudEvent = await request.ToCloudEventAsync(_jsonEventFormatter);
            
            // Determine if the event type is supported or not
            if (SupportedEventTypes.Contains(cloudEvent.Type) == false)
            {
                return null;
            }

            // Get data payload of our event to pass to durable entity
            return GetEventPayload<TPayload>(cloudEvent);
        }

        protected TEventPayload GetEventPayload<TEventPayload>(CloudEvent cloudEvent)
        {
            if (cloudEvent.Data is JObject == false)
            {
                throw new Exception("CloudEvent data was expected to be a JObject");
            }

            var dataObject = (JObject)cloudEvent.Data;
            return dataObject.ToObject<TEventPayload>();
        }
    }
}
