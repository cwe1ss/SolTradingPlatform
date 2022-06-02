using ConfigService.Models;
using Microsoft.AspNetCore.Mvc;

namespace ConfigService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoadBalancerController : ControllerBase
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

        // GET: api/LoadBalancer/{serviceType}
        [HttpGet("{serviceType}")]
        public ActionResult<string> Get(string serviceType)
        {
            List<Deployment> urlList = DeploymentList.Where(element => element.ServiceType == serviceType).ToList();
            if (urlList.Count > 0)
            {
                int index = Random.Shared.Next(urlList.Count);
                return urlList[index].URL;
            }
            else
            {
                return NotFound();
            }
        }
    }
}
