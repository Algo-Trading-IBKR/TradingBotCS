using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Util
{
    public interface Timezones
    {
        public static DateTime GetNewYorkTime()
        {
            //get datetime now in new york
            DateTime UtcTime = DateTime.UtcNow;
            TimeZoneInfo NYtimezoneinfo = TimeZoneInfo.FindSystemTimeZoneById("US Eastern Standard Time");
            DateTime NYtime = TimeZoneInfo.ConvertTimeFromUtc(UtcTime, NYtimezoneinfo);
            return NYtime;
        }

    }
}
