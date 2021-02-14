using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class BuyConfig
    {
        public bool BuyEnabled { get; set; }
        public bool UseTrailLimitOrders { get; set; }
        public float PriceOffset { get; set; }
        public float TrailingPercent { get; set; }
        public BuyConfig() { }
    }
}
