using MeiShop.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using MeiShop.Services;

namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashDeskController : ControllerBase
    {
        private readonly ILogger<CashDeskController> _logger;
        private readonly RoundRobinService _roundRobinService;

        public CashDeskController(ILogger<CashDeskController> logger, RoundRobinService roundRobinService)
        {
            _logger = logger;
            _roundRobinService = roundRobinService;
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

            //Mapping
            CreditcardTransaction creditCardTransaction = new CreditcardTransaction()
            {
                Amount = basket.AmountInEuro,
                CreditcardNumber = basket.CustomerCreditCardnumber,
                ReceiverName = basket.Vendor
            };

            int retries = 0;
            do
            {
                var creditcardServiceBaseAddress = _roundRobinService.GetBaseAddress();

                try
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(creditcardServiceBaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.PostAsJsonAsync(creditcardServiceBaseAddress + "/api/CreditcardTransactions", creditCardTransaction).Result;
                    response.EnsureSuccessStatusCode();
                    break;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error calling {ServiceUrl}", creditcardServiceBaseAddress);
                    retries++;
                }

            } while (retries < 3);

            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() }, creditCardTransaction);
        }
    }
}

