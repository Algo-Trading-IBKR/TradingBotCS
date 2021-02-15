using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;

namespace TradingBotCS.Controllers
{
    [ApiController]
    [Route("account")]
    public class AccountController : ControllerBase
    {
        public AccountController() { }

        [HttpGet]
        [Route("{key}/{allItems}")]
        public async Task<ActionResult> GetAccountInfo(string key, bool allItems = false)
        {
            List<AccountInfo> Result = await AccountRepository.ReadAccountUpdate(key, allItems);
            return new OkObjectResult(Result);
        }


    }
}
