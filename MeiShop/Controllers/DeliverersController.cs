using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DeliverersController : ControllerBase
    {
        private readonly ILogger<PaymentMethodsController> _logger;      
        private readonly IConfiguration _configuration;

        public DeliverersController(ILogger<PaymentMethodsController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            _logger.LogInformation("List Deliverers");

            using (HttpClient client = new HttpClient())
            {
                var deliverersFuncAddress = _configuration["DeliverersFuncAddress"];
                client.BaseAddress = new Uri(deliverersFuncAddress);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage response = await client.GetAsync(deliverersFuncAddress + "api/Deliverers");
                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var result = JsonConvert.DeserializeAnonymousType(content, new[] { new { name = "", price = 0.0 } });
                    return Ok(result);
                }
            }

            return Ok();
        }
    }
}
