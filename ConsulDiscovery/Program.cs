// See https://aka.ms/new-console-template for more information
using Consul;

Console.WriteLine("Hello, World!");

/*
     * https://www.consul.io/
     * CreditcardTransactions.json 
     * {
        "service":{"name": "Payment/CreditcardTransaction",
        "Address": "https://localhost",
        "tags": ["CreditCard","Transaction"],"port": 56093,
        "check":	 {
            "id": "HealthCheckCreditCardService",
            "name": "HTTP API on port 5000",
            "http": "http://localhost:56093/api/Healthcheck",  
            "interval": "10s",
            "timeout": "1s"
                }
            }
        }
    *c:\ consul agent -dev -enable-script-checks -config-dir=./config
    * http://localhost:8500/ui/dc1/services
     * 
     */

List<Uri> _serverUrls = new List<Uri>();
var consuleClient = new ConsulClient(c => c.Address = new Uri("http://127.0.0.1:8500"));
var services = consuleClient.Agent.Services().Result.Response;
foreach (var service in services)
{
    var isCreditCardApi = service.Value.Tags.Any(t => t == "CreditCard");
    if (isCreditCardApi)
    {
        try
        {
            var serviceUri = new Uri($"{service.Value.Address}:{service.Value.Port}");
            _serverUrls.Add(serviceUri);
        }
        catch (Exception)
        {

            ;
        }

    }
}
