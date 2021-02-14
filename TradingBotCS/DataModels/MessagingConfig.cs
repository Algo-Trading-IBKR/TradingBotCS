using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class MessagingConfig
    {
        public List<string> PhoneNumbers { get; set; }
        public List<string> MailAddresses { get; set; }
        public MessagingConfig() { }
    }
}
