using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;
using Microsoft.Extensions.Logging;

namespace TradingBotCS.Controllers
{
    [ApiController]
    [Route("config")]
    public class ConfigController : ControllerBase
    {

        public ConfigController() {}

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

    }
}
