using IEGEasyCreditCardService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IEGEasyCreditCardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditcardTransactionsController : ControllerBase
    {
        private readonly ILogger<CreditcardTransactionsController> _logger;

        public CreditcardTransactionsController(ILogger<CreditcardTransactionsController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public string Get(int id)
        {
            _logger.LogInformation("Hallo");
            return "value" + id;
        }
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] CreditcardTransaction creditcardTransaction)
        {
            _logger.LogInformation($"TransactionInfo Number: {creditcardTransaction.CreditcardNumber} Amount:{creditcardTransaction.Amount} Receiver: { creditcardTransaction.ReceiverName}");
            //log geht mit IEG
            return CreatedAtAction("Get", new { id = System.Guid.NewGuid() });
        }
    }
}
