﻿using Consul;
using MeiShop.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using MeiShop.Services;

namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CashDeskController : ControllerBase
    {
        //private static readonly string creditcardServiceBaseAddress = "https://iegeasycreditcardservice2022.azurewebsites.net/";

        private readonly ILogger<CashDeskController> _logger;
        private readonly IConfiguration _configuration;
        private readonly RoundRobinService _roundRobinService;

        public CashDeskController(ILogger<CashDeskController> logger, IConfiguration configuration, RoundRobinService roundRobinService)
        {
            _logger = logger;
            _configuration = configuration;
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

            //var creditcardServiceBaseAddress = _configuration["CreditcardServiceBaseAddress"];
            // var creditcardServiceBaseAddress = GetCreditCardTransactionsURIFromConsul().AbsolutePath;

            //Mapping
            CreditcardTransaction creditCardTransaction = new CreditcardTransaction()
            {
                Amount = basket.AmountInEuro,
                CreditcardNumber = basket.CustomerCreditCardnumber,
                CreditcardType = "American",
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

     

        private Uri GetCreditCardTransactionsURIFromConsul()
        {

            List<Uri> serverUrls = new List<Uri>();
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
                        serverUrls.Add(serviceUri);
                    }
                    catch (Exception)
                    {

                        ;
                    }

                }
            }
            return serverUrls.First();
        }
    }
}

