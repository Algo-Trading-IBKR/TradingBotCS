using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Util
{
    public  interface Converter
    {
        private static readonly string Name = "Converter";
        public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
        {
            // Unix timestamp is seconds past epoch
            System.DateTime dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, System.DateTimeKind.Utc);
            dtDateTime = dtDateTime.AddSeconds(unixTimeStamp).ToLocalTime();
            return dtDateTime;
        }

        public static async Task<dynamic> FixObjectValues(dynamic obj)
        {
            foreach (PropertyInfo prop in obj.GetType().GetProperties())
            {
                var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                if (type == typeof(double))
                {
                    try
                    {
                        if (Convert.ToDouble(prop.GetValue(obj, null)) >= Math.Pow(1.79, 100))
                        {
                            prop.SetValue(obj, 0.0);
                        }
                    }
                    catch (Exception e)
                    {
                        Logger.Warn(Name, $"{e} in object value fix");
                    }
                }
            }
            return obj;
        }
    }
}
