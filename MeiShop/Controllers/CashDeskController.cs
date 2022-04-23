using Consul;
using MeiShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;

namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashDeskController : ControllerBase
    {
        private readonly ILogger<CashDeskController> _logger;
        private static readonly string creditcardServiceBaseAddress = "https://iegeasycreditcardservice2022.azurewebsites.net/";
        private readonly IConfiguration Configuration;
        public CashDeskController(ILogger<CashDeskController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        [HttpGet]
        public IActionResult Get(string id)
        {
            return Content("OK");
        }

        [HttpPost]
        public IActionResult Post([FromBody] Basket basket)
        {
            _logger.LogInformation($"TransactionInfo Creditcard: {basket.CustomerCreditCardnumber} Product:{basket.Product} Amount: {basket.AmountInEuro}");

            var creditcardServiceBaseAddress = Configuration["CreditcardServiceBaseAddress"];
           // var creditcardServiceBaseAddress = GetCreditCardTransactionsURIFromConsul().AbsolutePath;
            //Mapping
            CreditcardTransaction creditCardTransaction = new CreditcardTransaction()
            {
                Amount = basket.AmountInEuro,
                CreditcardNumber = basket.CustomerCreditCardnumber,
                ReceiverName = basket.Vendor
            };

            HttpClient client = new HttpClient();
            client.BaseAddress = new Uri(creditcardServiceBaseAddress);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            HttpResponseMessage response = client.PostAsJsonAsync(creditcardServiceBaseAddress + "/api/CreditcardTransactions", creditCardTransaction).Result;
            response.EnsureSuccessStatusCode();


            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() }, creditCardTransaction);
        }

     

        private Uri GetCreditCardTransactionsURIFromConsul()
        {

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
            return _serverUrls.FirstOrDefault();
        }
    }
}

