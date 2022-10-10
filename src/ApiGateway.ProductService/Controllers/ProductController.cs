using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {

        [HttpGet]
        [Route("GetProducts")]
        public List<string> GetProducts()
        {
            var list = new List<string>();
            list.Add("Product 1");
            list.Add("Product 2");
            list.Add("Product 3");
            return list;
        }
    }
}
