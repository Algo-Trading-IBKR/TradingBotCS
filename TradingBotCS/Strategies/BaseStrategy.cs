using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Strategies
{
    public abstract class BaseStrategy
    {
        /// <summary>method <c>BuyStrategy</c> returns wether to buy or not and how many shares.</summary>
        public async Task<(bool, int)> BuyStrategy(string query)
        {
            return (false, 0);
        }

        /// <summary>method <c>BuyStrategy</c> returns wether to sell or not.</summary>
        public async Task<bool> SellStrategy()
        {
            return false;
        }

    }
}
