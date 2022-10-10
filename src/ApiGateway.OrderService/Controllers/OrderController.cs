using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiGateway.OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        [HttpGet]
        [Route("GetOrders")]
        public List<string> GetOrders()
        {
            var list = new List<string>();
            list.Add("Order 1");
            list.Add("Order 2");
            list.Add("Order 3");
            return list;
        }
    }
}
