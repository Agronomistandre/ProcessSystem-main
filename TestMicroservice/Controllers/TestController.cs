using Microsoft.AspNetCore.Mvc;

namespace TestMicroservice.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestController : ControllerBase
    {

        [HttpPost]
        public ActionResult Get([FromBody] string Value)
        {
            if (Value == null)
            {
                return BadRequest();
            }
            return Ok();
        }
    }
}