using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;


namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentMethodsController : ControllerBase
    {
        //https://docs.microsoft.com/en-us/aspnet/web-api/overview/advanced/calling-a-web-api-from-a-net-client
        private readonly ILogger<PaymentMethodsController> _logger;
      
        private readonly IConfiguration Configuration;


        public PaymentMethodsController(ILogger<PaymentMethodsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            List<string> acceptedPaymentMethods = new List<string>();
            _logger.LogInformation("Accepted Paymentmethods");
         

            using (HttpClient client = new HttpClient())
            {
                var creditcardServiceBaseAddress = Configuration["CreditcardServiceBaseAddress"];
                client.BaseAddress = new Uri(creditcardServiceBaseAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync(creditcardServiceBaseAddress + "/api/AcceptedCreditCards").Result;
                if (response.IsSuccessStatusCode)
                {
                    acceptedPaymentMethods = response.Content.ReadAsAsync<List<string>>().Result;
                }
            }

            if (acceptedPaymentMethods != null)
            {
                foreach (var item in acceptedPaymentMethods)
                {
                    _logger.LogInformation("Paymentmethod {0}", new object[] { item });

                }

            }
            return acceptedPaymentMethods;
        }
    }
}
