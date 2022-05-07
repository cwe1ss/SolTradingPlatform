using ConfigService.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ConfigService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadBalancer : ControllerBase
    {
        public static IList<Deployment> DeploymentList = new List<Deployment>()
        {
            new Deployment()
            {
                Id = 1,
                ServiceType = "FormDraftService",
                URL = "https://formdraftservice20220507143259.azurewebsites.net"
            },
            new Deployment()
            {
                Id = 2,
                ServiceType = "FormDraftService",
                URL = "https://formdraftservice20220507145531.azurewebsites.net"
            }
        };

        // GET: api/<LoadBalancer>
        [HttpGet("{ServiceType}")]
        public string Get(string ServiceType)
        {
            IList<Deployment> URL_list = DeploymentList.Where(element => element.ServiceType == ServiceType).ToList();
            var random = new Random();
            int index = random.Next(URL_list.Count);
            return URL_list[index].URL;
        }
    }
}
