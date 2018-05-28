using Microsoft.AspNetCore.Mvc;

namespace MutualFundPerformance.WebApi.Controllers
{

    [Route("api/[controller]")]
    public class PriceController : Controller
    {
        [HttpPost("GetAll")]
        public object[] GetAll(
            string symbol)
        {
            return new object[0];
        }

    }
}
