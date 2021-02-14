using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class ApiConfig
    {
        public string AccountId { get; set; }
        public string Ip { get; set; }
        public int Port { get; set; }
        public int ApiId { get; set; }
        public ApiConfig() { }
    }
}
