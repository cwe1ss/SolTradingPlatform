using Azure.Messaging.EventGrid;
using Azure.Messaging.EventGrid.SystemEvents;
using Microsoft.AspNetCore.Mvc;

namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WebhooksController : ControllerBase
    {
        // POST: /api/webhooks/handle_blob_creatd
        [HttpPost("handle_blob_created")] // <-- Must be an HTTP POST action
        public IActionResult ProcessAMSEvent(
            [FromBody] EventGridEvent[] ev, // 1. Bind the request
            [FromServices] ILogger<WebhooksController> logger)
        {
            // https://docs.microsoft.com/en-us/azure/event-grid/receive-events

            foreach (EventGridEvent eventGridEvent in ev)
            {
                // Handle system events
                if (eventGridEvent.TryGetSystemEventData(out object eventData))
                {
                    // Handle the subscription validation event
                    if (eventData is SubscriptionValidationEventData subscriptionValidationEventData)
                    {
                        logger.LogInformation($"Got SubscriptionValidation event data, validation code: {subscriptionValidationEventData.ValidationCode}, topic: {eventGridEvent.Topic}");
                        // Do any additional validation (as required) and then return back the below response

                        var responseData = new SubscriptionValidationResponse()
                        {
                            ValidationResponse = subscriptionValidationEventData.ValidationCode
                        };
                        return new OkObjectResult(responseData);
                    }
                    
                    if (eventData is StorageBlobCreatedEventData storageBlobCreatedEventData)
                    {
                        logger.LogInformation($"Got BlobCreated event data, blob URI {storageBlobCreatedEventData.Url}");

                    }
                }
            }

            return BadRequest();
        }
    }
}
