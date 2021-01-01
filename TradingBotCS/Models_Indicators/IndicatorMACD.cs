using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Models_Indicators;

namespace TradingBotCS.Models_Indicators
{
    public static class IndicatorMACD
    {
        // still need to add MACD histogram and signal line
        public static async Task<List<decimal>> MACD(List<decimal> data, int slowPeriod, int fastPeriod, int slowWeight = 2, int fastWeight = 2)
        {
            List<decimal> Result = new List<decimal>();
            List<decimal> SlowEma = await IndicatorMA.EMA(data, slowPeriod, slowWeight);
            List<decimal> FastEma = await IndicatorMA.EMA(data, fastPeriod, fastWeight);
            int periodDifference = slowPeriod - fastPeriod;

            for (int i = 0; i < (data.Count - slowPeriod + 1); i++)
            {
                Result.Add(FastEma[i+periodDifference] - SlowEma[i]);
            }
            return Result;
        }
        public static async Task<List<decimal>> MACD(List<int> data, int slowPeriod, int fastPeriod, int slowWeight = 2, int fastWeight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await MACD(DecList, slowPeriod, slowWeight, fastPeriod, fastWeight);
            return Result;
        }
        public static async Task<List<decimal>> MACD(List<float> data, int slowPeriod, int fastPeriod, int slowWeight = 2, int fastWeight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await MACD(DecList, slowPeriod, slowWeight, fastPeriod, fastWeight);
            return Result;
        }
        public static async Task<List<decimal>> MACD(List<double> data, int slowPeriod, int fastPeriod, int slowWeight = 2, int fastWeight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await MACD(DecList, slowPeriod, slowWeight, fastPeriod, fastWeight);
            return Result;
        }

        public static async Task<List<decimal>> MACDsignal(List<decimal> data, int period)
        {
            List<decimal> Result = await IndicatorMA.EMA(data, period);
            return Result;
        }
        public static async Task<List<decimal>> MACDsignal(List<int> data, int period)
        {
            List<decimal> Result = await IndicatorMA.EMA(data, period);
            return Result;
        }
        public static async Task<List<decimal>> MACDsignal(List<float> data, int period)
        {
            List<decimal> Result = await IndicatorMA.EMA(data, period);
            return Result;
        }
        public static async Task<List<decimal>> MACDsignal(List<double> data, int period)
        {
            List<decimal> Result = await IndicatorMA.EMA(data, period);
            return Result;
        }
        public static async Task<List<decimal>> MACDhist(List<decimal> macdData, List<decimal> macdSignalData)
        {
            List<decimal> Result = new List<decimal>();
            int signalOffset = macdData.Count - macdSignalData.Count;
            for (int i = 0; i < macdSignalData.Count; i++)
            {
                Result.Add(macdData[i + signalOffset] - macdSignalData[i]);
            }
            return Result;
        }

        public static async Task<List<decimal>> MACDhist(List<int> macdData, List<decimal> macdSignalData)
        {
            List<decimal> DecList = new List<decimal>();
            macdData.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await MACDhist(DecList, macdSignalData);
            return Result;
        }
        public static async Task<List<decimal>> MACDhist(List<float> macdData, List<decimal> macdSignalData)
        {
            List<decimal> DecList = new List<decimal>();
            macdData.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await MACDhist(DecList, macdSignalData);
            return Result;
        }
        public static async Task<List<decimal>> MACDhist(List<double> macdData, List<decimal> macdSignalData)
        {
            List<decimal> DecList = new List<decimal>();
            macdData.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await MACDhist(DecList, macdSignalData);
            return Result;
        }
    }
}
