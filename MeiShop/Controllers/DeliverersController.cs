using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Net.Http;
using Newtonsoft.Json;

namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliverersController : ControllerBase
    {
      
        private readonly ILogger<PaymentMethodsController> _logger;      
        private readonly IConfiguration Configuration;


        public DeliverersController(ILogger<PaymentMethodsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }
        [HttpGet]
        public IActionResult Get()
        {
            List<string> deliverers = new List<string>();
            _logger.LogInformation("List Deliverers");
         

            using (HttpClient client = new HttpClient())
            {
                var deliverersFuncAddress = Configuration["DeliverersFuncAddress"];
                client.BaseAddress = new Uri(deliverersFuncAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));


                HttpResponseMessage response = client.GetAsync(deliverersFuncAddress + "api/Deliverers").Result;
                if (response.IsSuccessStatusCode)
                {
                    var result = JsonConvert.DeserializeAnonymousType(response.Content.ReadAsStringAsync().Result, new[] { new { name = "",price=0.0 } });
                    return Ok(result);
                }
            }

            return Ok();
        }
    }
}
