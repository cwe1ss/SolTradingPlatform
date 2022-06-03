using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireAnswersService.Models;
using System.Text.Json;

namespace QuestionnaireAnswersService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireAnswersServiceController : ControllerBase
    {

        private static List<Questionnaire> _allQuestionaire = new List<Questionnaire>()
        {
            // new Questionaire()

        };
        private static List<Answer> _allAnswer = new List<Answer>()
        {
            // new Anwer()

        };
        private static List<Question> _allQuestion = new List<Question>()
        {
            // new Question()

        };

        private readonly IHttpClientFactory _httpClientFactory;

        public QuestionnaireAnswersServiceController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public object Get()
        {
            return _allQuestion;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Post([FromBody] Questionnaire questionnaire)
        {
            //Ausgefüllten Fragebogen speichern

            _allQuestionaire.Add(questionnaire);
            getAllAnswer(_allQuestionaire);

            // Create event message

            var questionnaireAnsweredEvent = new QuestionnaireAnsweredEvent
            {
                Questionnaire = questionnaire,
            };

            // Get Service Bus Connection string from our secret service

            var secretsClient = _httpClientFactory.CreateClient("SecretService");

            var secretResponse = await secretsClient.GetFromJsonAsync<SecretResponse>("/api/secrets/answers-queue")
                                 ?? throw new InvalidOperationException("couldn't load connection string from secret service");

            // Publish event to Service Bus queue
            // https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues

            var serviceBusClient = new ServiceBusClient(secretResponse.SecretValue);
            await using ServiceBusSender? serviceBusSender = serviceBusClient.CreateSender("answers");
      
            var jsonString = JsonSerializer.Serialize(questionnaireAnsweredEvent);

            var serviceBusMessage = new ServiceBusMessage(jsonString)
            {
                ContentType = "application/json",
                Subject = "questionnaire-answered",
                MessageId = Guid.NewGuid().ToString(),
            };

            await serviceBusSender.SendMessageAsync(serviceBusMessage);
            return NoContent();
        }

        private List<Question> getAllAnswer(List<Questionnaire> allQuestionaire)
        {
            foreach (Questionnaire questionaire in allQuestionaire)
            {
                _allQuestion.Add(questionaire.Questions.ElementAt(0));
            }
            return _allQuestion;
        }
    }
}
