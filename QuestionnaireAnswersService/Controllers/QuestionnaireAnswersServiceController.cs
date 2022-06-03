using Azure.Messaging.ServiceBus;
using Microsoft.AspNetCore.Mvc;
using QuestionnaireAnswersService.Models;
using SecretService.Models;
using System.Text.Json;


// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuestionnaireAnswersService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireAnswersServiceController : ControllerBase
    {

        private static List<Questionaire> _allQuestionaire = new List<Questionaire>()
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


        //public static string FormDraftServiceBaseAddress = "https://configservice20220507144709.azurewebsites.net/";

        // connection string to  Service Bus namespace
        static string connectionString = "Endpoint=sb://questionaire.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=urv9935Bu7PfYW7fxPvBQxr9GeP/dhCCKB3bW2zU7zU=";
        // name of the Service Bus queue

        [HttpGet]
        public object Get()
        {
            return _allQuestion;
        }


        [HttpPost]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<ActionResult> Post([FromBody] Questionaire questionaire)
        {
            //Ausgefüllten Fragebogen speichern 
            _allQuestionaire.Add(questionaire);
            getAllAnswer(_allQuestionaire);



            //send event to FragebogenAusgefülltEvent 

            // Create event message

            var questionaireEventMessage = new Questionaire
            {
                QuestionaireId = questionaire.QuestionaireId,
                Name = questionaire.Name,
                Status = questionaire.Status,
                Description = questionaire.Description,
          

            };

            // Publish event to Service Bus queue
            // https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues
            var serviceBusClient = new ServiceBusClient(connectionString);
            await using ServiceBusSender? serviceBusSender = serviceBusClient.CreateSender("answers");
      
            var jsonString = JsonSerializer.Serialize(questionaireEventMessage);

            var serviceBusMessage = new ServiceBusMessage(jsonString)
            {
                ContentType = "application/json",
                Subject = "questionnaire_answered",
                MessageId = Guid.NewGuid().ToString(),
            };

            await serviceBusSender.SendMessageAsync(serviceBusMessage);
            return NoContent();


        }

        private List<Question> getAllAnswer(List<Questionaire> allQuestionaire)
        {

            foreach (Questionaire questionaire in allQuestionaire)
            {
                _allQuestion.Add(questionaire.Questions.ElementAt(0));
            }
            return _allQuestion;
        }
    }
}
