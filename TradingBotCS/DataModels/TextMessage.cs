using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class TextMessage
    {
        public string Channel { get; set; }
        public string To { get; set; }
        public string Content { get; set; }

        public TextMessage(string content, string channel = "sms", string to = "32476067619")
        {
            Content = content;
            Channel = channel;
            To = to;
        }
    }
}
