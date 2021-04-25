using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingBotCS.Database;

namespace TradingBotCS.Util
{
    public class DataManager
    {
        private static readonly string Name = "Data Manager";

        public static async Task ReadRawData(List<Symbol> symbols)
        {
            Logger.Verbose(Name, "Reading Data From Database");
            foreach (Symbol S in symbols)
            {
                Program.IbClient.ClientSocket.cancelRealTimeBars(S.Id);
                S.RawDataList = await RawDataRepository.ReadRawData(S.Ticker, 30000);
            }
        }

        public static async Task GetRealTimeData(List<Symbol> symbolObjects, string generickTickList = "")
        {
            if (symbolObjects.Count > 100) Logger.Warn(Name, $"Maximum allowed data request is 100, length of list is {symbolObjects.Count}"); else Logger.Info(Name, $"Length of Symbol list: {symbolObjects.Count}");
            foreach (Symbol S in symbolObjects)
            {
                //Program.IbClient.ClientSocket.reqMarketDataType(4); // verwijderen voor live data of naar 1 veranderen
                Program.IbClient.ClientSocket.reqMktData(S.Id, S.Contract, generickTickList, false, false, null);
            }
        }

        public static async Task GetRealTimeBars(List<Symbol> symbolObjects, int barSize = 5, string type = "MIDPOINT", bool useRTH = true)
        {
            if (symbolObjects.Count > 100) Logger.Warn(Name, $"Maximum allowed data request is 100, length of list is {symbolObjects.Count}"); else Logger.Info(Name, $"Length of Symbol list: {symbolObjects.Count}");
            foreach (Symbol S in symbolObjects)
            {
                //Program.IbClient.ClientSocket.cancelRealTimeBars(S.Id);
                Program.IbClient.ClientSocket.reqRealTimeBars(S.Id, S.Contract, barSize, type, useRTH, null);
            }
        }

        public static async Task GetHistoricalBars(List<Symbol> symbolObjects, DateTime endTime, int startingHour = 0, int endingHour = 0, int startingMinute = 0, int endingMinute = 0, string duration = "5 D", string barSize = "15 mins", string whatToShow = "MIDPOINT", int useRTH = 1, int formatDate = 1, bool keepUpToDate = false)
        {
            string QueryTime;
            bool CustomTimeFrame = false;

            if (endTime == DateTime.UnixEpoch)
            {
                QueryTime = Timezones.GetNewYorkTime().ToString("yyyyMMdd HH:mm:ss");
            }
            else
            {
                QueryTime = endTime.ToString("yyyyMMdd HH:mm:ss");
            }
            if (startingHour != 0 && endingHour != 0 && startingMinute != 0 && endingMinute != 0)
            {
                CustomTimeFrame = true;
            }

            new Thread(() =>
            {
                DateTime NYtime = Timezones.GetNewYorkTime();
                while (CustomTimeFrame && (startingHour > NYtime.Hour || startingMinute > NYtime.Minute))
                {
                    NYtime = Timezones.GetNewYorkTime();
                }
                foreach (Symbol S in symbolObjects)
                {
                    try
                    {
                    //S.RawDataList = await RawDataRepository.ReadRawData(S.Ticker);
                    if (Program.GettingHistoricalData < 50)
                    {
                        while (Program.GettingHistoricalData >= 49)
                        {
                            Thread.Sleep(1);
                            if (S == symbolObjects.Last()) break;
                            if (CustomTimeFrame == true && (endingHour < NYtime.Hour && endingMinute < NYtime.Minute)) break;
                            };
                        Program.IbClient.ClientSocket.reqHistoricalData(S.Id, S.Contract, QueryTime, duration, barSize, whatToShow, useRTH, formatDate, keepUpToDate, null); // maar 50 tegelijk
                        Program.GettingHistoricalData += 1;
                        Thread.Sleep(20);
                        }
                    }
                    catch (Exception ex)
                    {
                        Logger.Error(Name, $"{S.Ticker} Failed: \n{ex}");
                    }
                }
                if (Program.GettingHistoricalData == 0)
                {
                    Logger.Info(Name, "Historical Data Done");
                }
            })
            { IsBackground = false }.Start();
        }

    }
}
