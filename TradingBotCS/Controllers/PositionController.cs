using Microsoft.AspNetCore.Mvc;
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
    [Route("position")]
    public class PositionController : ControllerBase
    {
        public PositionController() { }

        [HttpGet]
        [Route("all")]
        public async Task<IActionResult> GetAllPositions()
        {
            try
            {
                List<Position> Results = await PositionsRepository.ReadPositions(allItems: true);
                return new OkObjectResult(Results);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

        [HttpGet]
        [Route("{symbol}")]
        public async Task<IActionResult> GetPosition(string symbol)
        {
            try
            {
                Console.WriteLine(symbol);
                List<Position> Results = await PositionsRepository.ReadPositions(symbol);
                return new OkObjectResult(Results);
            }
            catch (Exception)
            {
                return new StatusCodeResult(500);
            }
        }

    }
}