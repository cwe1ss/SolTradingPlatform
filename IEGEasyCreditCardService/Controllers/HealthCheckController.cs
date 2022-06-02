using Microsoft.AspNetCore.Mvc;

namespace IEGEasyCreditCardService.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class HealthCheckController : ControllerBase
    {

        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            return Ok();
        }

    }
}
