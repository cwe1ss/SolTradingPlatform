using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ProductCatalogService.Models;

namespace ProductCatalogService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductListController : ControllerBase
    {
        private static List<Product> _allProducts = new List<Product>()
        {
            new Product(){ProductId=1,ProductName="Kürbiskernöl"},
            new Product(){ProductId=2,ProductName="Schloßbergtropferl",}
        };

        [HttpGet]
        [EnableQuery]
        public List<Product> Get()
        {
            return _allProducts;
        }
    }
}
