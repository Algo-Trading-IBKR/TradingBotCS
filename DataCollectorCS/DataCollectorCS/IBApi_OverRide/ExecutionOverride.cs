using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.IBApi_OverRide
{
    public class ExecutionOverride : Execution
    {
        private static string Name = "ExecutionOverride";

        public DateTime DateTime { get; set; }

        public string Symbol { get; set; }

        public string Action { get; set; }

        public string OrderType { get; set; }

        public ExecutionOverride(Execution execution)
        {
            try
            {
                DateTime = DateTime.Now;
                OrderId = execution.OrderId;
                ClientId = execution.ClientId;
                ExecId = execution.ExecId;
                Time = execution.Time;
                AcctNumber = execution.AcctNumber;
                Exchange = execution.Exchange;
                Side = execution.Side;
                Shares = execution.Shares;
                Price = execution.Price;
                PermId = execution.PermId;
                Liquidation = execution.Liquidation;
                CumQty = execution.CumQty;
                AvgPrice = execution.AvgPrice;
                OrderRef = execution.OrderRef;
                EvRule = execution.EvRule;
                EvMultiplier = execution.EvMultiplier;
                ModelCode = execution.ModelCode;
                LastLiquidity = execution.LastLiquidity;
            }
            catch (Exception ex)
            {
                Logger.Error(Name, $"{ex}");
            }
        }
    }
}
