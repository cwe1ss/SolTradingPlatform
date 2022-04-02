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
        private static Dictionary <String,CreditcardTransaction> _creditcardTransactions = new();

        public CreditcardTransactionsController(ILogger<CreditcardTransactionsController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        public object Get(string id)
        {
            //_logger.LogInformation("Hallo");
            return _creditcardTransactions[id];
        }
        // POST api/values
        [HttpPost]
        public IActionResult Post([FromBody] CreditcardTransaction creditcardTransaction)
        {
            _logger.LogInformation($"TransactionInfo Number: {creditcardTransaction.CreditcardNumber} Amount:{creditcardTransaction.Amount} Receiver: { creditcardTransaction.ReceiverName}");
            //log geht mit IEG
            string id = System.Guid.NewGuid().ToString();
            _creditcardTransactions.Add(id,creditcardTransaction);
            return CreatedAtAction("Get", new { id });
        }
    }
}
