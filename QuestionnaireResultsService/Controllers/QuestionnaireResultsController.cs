using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace QuestionnaireResultsService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionnaireResultsController : ControllerBase
    {
        // connection string to  Service Bus namespace
        static string connectionString = "<NAMESPACE CONNECTION STRING>";
        // name of the Service Bus queue
        static string queueName = "<QUEUE NAME>";
        // the processor that reads and processes messages from the queue
        static ServiceBusProcessor processor;

        // handle received messages
        static async Task MessageHandler(ProcessMessageEventArgs args)
        {
            string body = args.Message.Body.ToString();
            Console.WriteLine($"Received: {body}");

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }

        // handle any errors when receiving messages
        static Task ErrorHandler(ProcessErrorEventArgs args)
        {
            Console.WriteLine(args.Exception.ToString());
            return Task.CompletedTask;
        }

        // GET api/<QuestionnaireResultsController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<QuestionnaireResultsController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
            //-------------------------------------------
           

            // Receive event from Service Bus queue
            // https://docs.microsoft.com/en-us/azure/service-bus-messaging/service-bus-dotnet-get-started-with-queues
            var serviceBusClient = new ServiceBusClient(connectionString);
            // create a processor that we can use to process the messages
            processor = serviceBusClient.CreateProcessor(queueName, new ServiceBusProcessorOptions());
            try
            {
                // add handler to process messages
                processor.ProcessMessageAsync += MessageHandler;

                // add handler to process any errors
                processor.ProcessErrorAsync += ErrorHandler;

                // start processing 
               // await processor.StartProcessingAsync();

                Console.WriteLine("Wait for a minute and then press any key to end the processing");
                Console.ReadKey();

                // stop processing 
                Console.WriteLine("\nStopping the receiver...");
               // await processor.StopProcessingAsync();
                Console.WriteLine("Stopped receiving messages");
            }
            finally
            {
                // Calling DisposeAsync on client types is required to ensure that network
                // resources and other unmanaged objects are properly cleaned up.
               // await processor.DisposeAsync();
              //  await client.DisposeAsync();
            }
        }
    }

       
    
}
