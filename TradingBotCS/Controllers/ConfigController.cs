using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;


namespace TradingBotCS.Controllers
{
    [ApiController]
    [Route("Config")]
    public class ConfigController : ControllerBase
    {

        private readonly ILogger<ConfigController> Logger;
        public ConfigController(ILogger<ConfigController> logger) {
            Logger = logger;
            Logger.LogInformation("ctor");
        }

        [HttpGet]
        public async Task<ActionResult> GetConfig()
        {
            Configuration Config = await ConfigRepository.ReadConfig();
            return new OkObjectResult(Config);
        }

        [HttpPost]
        public async Task<ActionResult> UpsertConfig(Configuration config)
        {
            try
            {
                ConfigRepository.UpsertConfig(config);
                Program.UpdateConfigs();
                return new OkObjectResult(config);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
            
        }

        //[HttpGet]
        //[Route("account/{key}")]
        //public async Task<ActionResult> GetAccountInfo(string key)
        //{
        //    List<AccountInfo> result = await AccountRepository.ReadAccountUpdate(key);
        //    return new OkObjectResult(result);
        //}



    }
}
