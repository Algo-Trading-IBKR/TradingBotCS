using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class TextMessage
    {
        public string channel { get; set; }
        public string to { get; set; }
        public string content { get; set; }

        public TextMessage(string parcontent, string parchannel = "sms", string parto = "32476067619")
        {
            content = parcontent;
            channel = parchannel;
            to = parto;
        }
    }
}
