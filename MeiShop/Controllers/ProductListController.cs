using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MeiShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductListController : ControllerBase
    {
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "Kürbiskernöl", "Schloßbergtropferl" };
        }
    }
}
