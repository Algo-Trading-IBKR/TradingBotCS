using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "ApiConfig is missing")]
        public ApiConfig ApiConfig { get; set; }

        [Required(ErrorMessage = "StrategyConfig is missing")]
        public StrategyConfig StrategyConfig { get; set; }

        [Required(ErrorMessage = "BuyConfig is missing")]
        public BuyConfig BuyConfig { get; set; }

        [Required(ErrorMessage = "SellConfig is missing")]
        public SellConfig SellConfig { get; set; }

        [Required(ErrorMessage = "ErrorCodeConfig is missing")]
        public ErrorCodeConfig ErrorCodeConfig { get; set; }

        [Required(ErrorMessage = "MessagingConfig is missing")]
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

