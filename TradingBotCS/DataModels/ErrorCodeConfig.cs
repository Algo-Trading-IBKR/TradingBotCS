using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class ErrorCodeConfig
    {
        public List<int> InfoCodes { get; set; }
        public List<int> WarningCodes { get; set; }
        public ErrorCodeConfig() { }
    }
}
