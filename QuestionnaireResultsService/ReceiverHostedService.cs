using Azure.Messaging.ServiceBus;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using QuestionnaireResultsService.Models;

namespace QuestionnaireResultsService;

public class ReceiverHostedService : BackgroundService
{
    private readonly ILogger<ReceiverHostedService> _logger;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly TelemetryClient _telemetryClient;

    // the client that owns the connection and can be used to create senders and receivers
    private ServiceBusClient? _serviceBusClient;

    // the processor that reads and processes messages from the queue
    private ServiceBusProcessor? _serviceBusProcessor;

    public ReceiverHostedService(ILogger<ReceiverHostedService> logger, IHttpClientFactory httpClientFactory, TelemetryClient telemetryClient)
    {
        _logger = logger;
        _httpClientFactory = httpClientFactory;
        _telemetryClient = telemetryClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // Get Service Bus Connection string from our secret service

        var secretsClient = _httpClientFactory.CreateClient("SecretService");

        var secretResponse = await secretsClient.GetFromJsonAsync<SecretResponse>("/api/secrets/answers-queue", stoppingToken)
                             ?? throw new InvalidOperationException("couldn't load connection string from secret service");

        // The Service Bus client types are safe to cache and use as a singleton for the lifetime
        // of the application, which is best practice when messages are being published or read
        // regularly.
        //

        // Create the client object that will be used to create sender and receiver objects
        _serviceBusClient = new ServiceBusClient(secretResponse.SecretValue);

        // create a processor that we can use to process the messages
        _serviceBusProcessor = _serviceBusClient.CreateProcessor("answers", new ServiceBusProcessorOptions());

        _serviceBusProcessor.ProcessMessageAsync += MessageHandler;

        _serviceBusProcessor.ProcessErrorAsync += ErrorHandler;

        // start processing 
        await _serviceBusProcessor.StartProcessingAsync(stoppingToken);

        _logger.LogInformation("ServiceBusProcessor started");

        stoppingToken.Register(() =>
        {
            _logger.LogInformation("ServiceBusProcessor stopping");
            _serviceBusProcessor.StopProcessingAsync().GetAwaiter().GetResult();

            // Calling DisposeAsync on client types is required to ensure that network
            // resources and other unmanaged objects are properly cleaned up.
            _serviceBusProcessor.DisposeAsync().GetAwaiter().GetResult();
            _serviceBusClient.DisposeAsync().GetAwaiter().GetResult();
        });
    }

    // handle received messages
    async Task MessageHandler(ProcessMessageEventArgs args)
    {
        // Show each receive as a REQUEST in Application Insights
        // https://docs.microsoft.com/en-us/azure/azure-monitor/app/custom-operations-tracking

        var requestTelemetry = new RequestTelemetry {Name = "MSG answers-queue"};
        requestTelemetry.Context.Operation.Id = args.Message.MessageId;

        var operation = _telemetryClient.StartOperation(requestTelemetry);
        try
        {
            // Process the actual message

            string body = args.Message.Body.ToString();

            _logger.LogInformation("Received Message {Body}", body);

            // complete the message. message is deleted from the queue. 
            await args.CompleteMessageAsync(args.Message);
        }
        catch (Exception ex)
        {
            _telemetryClient.TrackException(ex);
            throw;
        }
        finally
        {
            _telemetryClient.StopOperation(operation);

            // For demo purposes: Make sure we don't have to wait long for it to pop up in App Insights
            _telemetryClient.Flush();
        }
    }

    // handle any errors when receiving messages
    Task ErrorHandler(ProcessErrorEventArgs args)
    {
        _logger.LogError(args.Exception, "Error on receive!");
        return Task.CompletedTask;
    }
}