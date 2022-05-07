using Microsoft.AspNetCore.Mvc;
using Paymentservice.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Paymentservice.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaypalPaymentTransactionController : ControllerBase
    {
        private readonly ILogger<PaypalPaymentTransactionController> _logger;
        public PaypalPaymentTransactionController(ILogger<PaypalPaymentTransactionController> logger)
        {
            _logger = logger;
        }
        // GET: api/<PaypalPaymentTransactionController>
        // [HttpGet]
        [HttpGet(Name = "GetPaypalPaymentTransaction")]
        public string Get(int id)
        {
            _logger.LogInformation("Paypal payment");
             return "value" + id;

        }

        // POST api/<PaypalPaymentTransactionController>
        [HttpPost]
      
        public IActionResult Post([FromBody] PaypalPaymentTransaction paypalPaymentTransaction)
        {
            _logger.LogInformation($"TransactionInfo Username: {paypalPaymentTransaction.PayPalUsername} Amount:{paypalPaymentTransaction.Amount} Password: { paypalPaymentTransaction.PayPalPasswort}");

            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() });
        }

     

        }
    }

