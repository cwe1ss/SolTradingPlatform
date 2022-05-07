using System.Text.Json;
using Azure.Messaging.ServiceBus;
using FormPublishService.Models;
using Microsoft.AspNetCore.Mvc;

namespace FormPublishService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        private static readonly HashSet<string> PublishedFormIds = new();

        public PublishController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        
        // POST api/publish
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Post(string formId)
        {
            // Already published?

            if (PublishedFormIds.Contains(formId))
                return NoContent();

            // -------------------------------------
            // Publish form

            //var formDraftClient = _httpClientFactory.CreateClient("FormDraftService");
            //await formDraftClient.PostAsJsonAsync("/api/")
            PublishedFormIds.Add(formId);

            // -------------------------------------
            // Create event message

            var formPublishedEvent = new FormPublishedEvent
            {
                FormId = formId,
            };

            // -------------------------------------
            // Get Service Bus Connection string from our secret service

            var secretsClient = _httpClientFactory.CreateClient("SecretService");

            var secretResponse = await secretsClient.GetFromJsonAsync<SecretResponse>("/api/secrets/form-published-queue")
                ?? throw new InvalidOperationException("couldn't load connection string from secret service");

            // -------------------------------------
            // Publish event to Service Bus queue
            // https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues

            var serviceBusClient = new ServiceBusClient(secretResponse.SecretValue);

            await using ServiceBusSender? serviceBusSender = serviceBusClient.CreateSender("form-published");

            var jsonString = JsonSerializer.Serialize(formPublishedEvent);

            var serviceBusMessage = new ServiceBusMessage(jsonString)
            {
                ContentType = "application/json",
                Subject = "form-published",
                MessageId = Guid.NewGuid().ToString(),
            };

            await serviceBusSender.SendMessageAsync(serviceBusMessage);

            return NoContent();
        }
    }
}
