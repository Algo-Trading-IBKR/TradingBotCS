using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class Configuration
    {
        public ObjectId _id { get; set; }
        public DateTime DateTime { get; set; }
        public string Name { get; set; }

        public ApiConfig ApiConfig { get; set; }
        public StrategyConfig StrategyConfig { get; set; }
        public BuyConfig BuyConfig { get; set; }
        public SellConfig SellConfig { get; set; }
        public ErrorCodeConfig ErrorCodeConfig { get; set; }
        public MessagingConfig MessagingConfig { get; set; }

        public Configuration()
        {
            DateTime = DateTime.Now;
            Name = "Main";
            ApiConfig = new ApiConfig();
            BuyConfig = new BuyConfig();
            ErrorCodeConfig = new ErrorCodeConfig();
            MessagingConfig = new MessagingConfig();
            SellConfig = new SellConfig();
            StrategyConfig = new StrategyConfig();
        }
    }
}

