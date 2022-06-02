using IEGEasyCreditCardService.Models;
using Microsoft.AspNetCore.Mvc;

namespace IEGEasyCreditCardService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CreditcardTransactionsController : ControllerBase
    {
        private static readonly Dictionary <string,CreditcardTransaction> CreditcardTransactions = new();

        private readonly ILogger<CreditcardTransactionsController> _logger;

        public CreditcardTransactionsController(ILogger<CreditcardTransactionsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public ActionResult<CreditcardTransaction> Get(string id)
        {
            _logger.LogInformation("transaction {id} requested", id);

            if (CreditcardTransactions.TryGetValue(id, out var transaction))
            {
                return transaction;
            }
            else
            {
                return NotFound();
            }
        }

        [HttpPost]
        public IActionResult Post([FromBody] CreditcardTransaction creditcardTransaction)
        {
            _logger.LogInformation(
                "TransactionInfo Number: {CreditcardNumber}; Amount: {Amount}; Receiver: {ReceiverName}",
                creditcardTransaction.CreditcardNumber, creditcardTransaction.Amount, creditcardTransaction.ReceiverName);
            
            string id = Guid.NewGuid().ToString();
            CreditcardTransactions.Add(id,creditcardTransaction);

            return CreatedAtAction("Get", new { id }, creditcardTransaction);
        }
    }
}
