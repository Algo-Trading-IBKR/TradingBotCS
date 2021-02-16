using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Controllers
{
    [ApiController]
    [Route("login")]
    public class LoginController : ControllerBase
    {
        public LoginController() { }

        [HttpPost]
        public async Task<ActionResult> Login([FromBody] string content)
        {
            Console.WriteLine(content);
            return new OkObjectResult(content);
        }
    }
}
