using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ecommerce.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("{id}/{name?}")]
        public IActionResult Testt(int id, string? name,IFormFile formFile)
        {
            // Your action code
            return Ok();
        }
    }
}
