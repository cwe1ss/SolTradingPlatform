using Azure.Messaging.ServiceBus;
using FormDraftService.Models;
using Microsoft.AspNetCore.Mvc;
using SecretService.Models;
using System.Text.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuestionnaireAnswersService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireAnswersServiceController : ControllerBase
    {

        private static List<Questionaire> _allFormDraft = new List<Questionaire>()
        {
           // new FormDraft()
           
        };
      
        public static string FormDraftServiceBaseAddress = "https://configservice20220507144709.azurewebsites.net/";

        // connection string to  Service Bus namespace
        static string connectionString = "Endpoint=sb://questionaire.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=urv9935Bu7PfYW7fxPvBQxr9GeP/dhCCKB3bW2zU7zU=";
        // name of the Service Bus queue

        [HttpGet("{serviceType}")]
        public IActionResult Get(string serviceType)
        {
            //FormDraftService
            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(FormDraftServiceBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.GetAsync(FormDraftServiceBaseAddress + "/api/LoadBalancer/"+ serviceType).Result;
            response.EnsureSuccessStatusCode();

            return Ok(response.Content.ReadAsStringAsync().Result);


        }
        [HttpGet]
        public object Get()
        {
            return _allFormDraft;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Post([FromBody] Questionaire formDraft)
        {
            //Ausgefüllten Fragebogen speichern 
            _allFormDraft.Add(formDraft);
    
            //send event to FragebogenAusgefülltEvent 
           
            // Create event message

            var formDraftEventMessage = new Questionaire
            {
                FormId = formDraft.FormId
            };

            // Get Service Bus Connection string from our secret service mit Christian besprochen
  
            //var secretsClient = _httpClientFactory.CreateClient("SecretService");

         //   var secretResponse = await secretsClient.GetFromJsonAsync<SecretResponse>("/api/secrets/form-published-queue") // was muss ich hier genau eingeben 
         //       ?? throw new InvalidOperationException("couldn't load connection string from secret service");

            // Publish event to Service Bus queue
            // https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues
            var serviceBusClient = new ServiceBusClient(connectionString);
            await using ServiceBusSender? serviceBusSender = serviceBusClient.CreateSender("answers");
      
            var jsonString = JsonSerializer.Serialize(formDraftEventMessage);

            var serviceBusMessage = new ServiceBusMessage(jsonString)
            {
                ContentType = "application/json",
                Subject = "questionnaire_answered",
                MessageId = Guid.NewGuid().ToString(),
            };

            await serviceBusSender.SendMessageAsync(serviceBusMessage);

            //return Ok(formDraft);
            return NoContent();


        }

    }
}
