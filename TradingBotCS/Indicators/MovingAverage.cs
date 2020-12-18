using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestApp.Classes
{
    public static class MovingAverage
    {
        // Simple Moving Average
        public static async Task<List<decimal>> SMA(List<decimal> data, int period, bool equalLength = true)
        {
            List<decimal> Result = new List<decimal>();
            decimal Sma = new decimal();
            int MaCount = data.Count - period;
            if (equalLength == true){
                for (int x = 0; x < period - 1; x++)
                {
                    Result.Add(0);
                }
            }
            for (int i = 0; i <= MaCount; i++)
            {
                Sma = 0;
                foreach (decimal Datapoint in data.GetRange(i, period))
                {
                    Sma += Datapoint;
                }
                Result.Add(Sma/period);
            }
            return Result;
        }
        public static async Task<List<decimal>> SMA(List<int> data, int period, bool equalLength = true)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await SMA(DecList, period, equalLength);
            return Result;
        }
        public static async Task<List<decimal>> SMA(List<float> data, int period, bool equalLength = true)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await SMA(DecList, period, equalLength);
            return Result;
        }
        public static async Task<List<decimal>> SMA(List<double> data, int period, bool equalLength = true)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await SMA(DecList, period, equalLength);
            return Result;
        }

        // Exponential Moving Average: {Close - EMA(previous day)} x K + EMA(previous day).

        public static async Task<List<decimal>> EMA(List<decimal> data, int period, decimal weight = 2)
        {
            List<decimal> Result = new List<decimal>();
            decimal K = weight / ((decimal)period + 1);
            int MaCount = data.Count - period;
            decimal Ema = new decimal();

            foreach (decimal Datapoint in data.GetRange(0, period))Ema += Datapoint;
            Ema = Ema/period;
            Result.Add(Ema);
            for (int i = 0; i < MaCount; i++)
            {
                Ema = (data[i+period] - Ema) * K + Ema;
                Result.Add(Ema);
            }
            
            return Result;
        }
        public static async Task<List<decimal>> EMA(List<int> data, int period, decimal weight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await EMA(DecList, period, weight);
            return Result;
        }
        public static async Task<List<decimal>> EMA(List<float> data, int period, decimal weight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await EMA(DecList, period, weight);
            return Result;
        }
        public static async Task<List<decimal>> EMA(List<double> data, int period, decimal weight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await EMA(DecList, period, weight);
            return Result;
        }


        // Double Exponential Moving Average = (2 x EMA1) - EMA2
        // EMA2: eerste ema nog eens door ema sturen
        public static async Task<List<decimal>> DEMA(List<decimal> data, int period, decimal weight = 2)
        {
            List<decimal> EmaResult = await EMA(data, period, weight);
            List<decimal> DoubleEmaResult = await EMA(EmaResult, period, weight);
            List<decimal> Result = new List<decimal>();

            decimal Dema = new decimal();
            int MaCount = data.Count - 1;
            
            Result.Add(data[0]);

            for (int i = 1; i <= MaCount; i++)
            {
                Dema = (EmaResult[i] * 2) - DoubleEmaResult[i];
                Result.Add(Dema);
            }
            return Result;
        }
        public static async Task<List<decimal>> DEMA(List<int> data, int period, decimal weight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await DEMA(DecList, period, weight);
            return Result;
        }
        public static async Task<List<decimal>> DEMA(List<float> data, int period, decimal weight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await DEMA(DecList, period, weight);
            return Result;
        }
        public static async Task<List<decimal>> DEMA(List<double> data, int period, decimal weight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await DEMA(DecList, period, weight);
            return Result;
        }

        // Kaufman Adaptive Moving Average
        public static async Task<List<decimal>> KAMA(List<decimal> data, int erPeriod, decimal fastSC, decimal slowSC)
        {
            List<decimal> Result = new List<decimal>();

            List<decimal> ErList = new List<decimal>();

            decimal Change = new decimal();
            decimal Volatility = new decimal();
            decimal EfficiencyRatio = new decimal();

            List<decimal> SCList = new List<decimal>();

            decimal KamaItem = data[erPeriod-1];
            Result.Add(KamaItem);

            for (int i = 0; i<data.Count-erPeriod; i++)
            {
                Change = Math.Abs(data[i+erPeriod] - data[i]);
                Volatility = 0;
                for (int j = 1; j <= erPeriod; j++)
                {
                    Volatility += Math.Abs(data[j+i] - data[j+i - 1]);
                }
                EfficiencyRatio = Change / Volatility;
                ErList.Add(EfficiencyRatio);
                decimal SC = (decimal)Math.Pow((double)(EfficiencyRatio * (2 / (fastSC + 1) - 2 / (slowSC + 1)) + 2 / (slowSC + 1)), 2);
                SCList.Add(SC);

                KamaItem = KamaItem + SC * (data[i + erPeriod] - KamaItem);
                Result.Add(KamaItem);
            }

            //Console.ReadKey();

            return Result;
        }

        //public static async Task<List<decimal>> KAMA(List<int> data, int period, decimal weight = 2)
        //{
        //    List<decimal> DecList = new List<decimal>();
        //    data.ForEach(item => DecList.Add((decimal)item));
        //    List<decimal> Result = await DEMA(DecList, period, weight);
        //    return Result;
        //}
        //public static async Task<List<decimal>> KAMA(List<float> data, int period, decimal weight = 2)
        //{
        //    List<decimal> DecList = new List<decimal>();
        //    data.ForEach(item => DecList.Add((decimal)item));
        //    List<decimal> Result = await DEMA(DecList, period, weight);
        //    return Result;
        //}
        //public static async Task<List<decimal>> KAMA(List<double> data, int period, decimal weight = 2)
        //{
        //    List<decimal> DecList = new List<decimal>();
        //    data.ForEach(item => DecList.Add((decimal)item));
        //    List<decimal> Result = await DEMA(DecList, period, weight);
        //    return Result;
        //}

        // still need to add MACD histogram and signal line
        public static async Task<List<decimal>> MACD(List<decimal> data,  int slowPeriod, int fastPeriod, int slowWeight = 2, int fastWeight = 2)
        {
            List<decimal> Result = new List<decimal>();
            List<decimal> SlowEma = await EMA(data, slowPeriod, slowWeight);
            List<decimal> FastEma = await EMA(data, fastPeriod, fastWeight);

            for ( int i = fastPeriod; i < (data.Count - fastPeriod -1); i++ )
            {
                Result.Add(FastEma[i + 2] - SlowEma[i - fastPeriod]);
            }
            return Result;
        }
        public static async Task<List<decimal>> MACD(List<int> data, int slowPeriod, int fastPeriod, int slowWeight = 2, int fastWeight = 2)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await MACD(DecList, slowPeriod, slowWeight, fastPeriod, fastWeight );
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
            List<decimal> Result = await EMA(data, period);
            return Result;
        }
        public static async Task<List<decimal>> MACDhist(List<decimal> macdData, List<decimal> macdSignalData)
        {
            List<decimal> Result = new List<decimal>();
            int signalOffset = macdData.Count - macdSignalData.Count;
            for (int i = 0; i < macdSignalData.Count; i++)
            {
                //Console.WriteLine(macdData[i + signalOffset]);
                //Console.WriteLine(macdSignalData[i]);
                //Console.ReadKey();
                Result.Add(macdData[i + signalOffset] - macdSignalData[i]);
            }
            return Result;
        }

        public static async Task<List<decimal>> RSI(List<decimal> data, int period)
        {
            List<decimal> Result = new List<decimal>();
            List<decimal> GainList = new List<decimal>();
            List<decimal> LossList = new List<decimal>();
            decimal AvgGain = new decimal();
            decimal AvgLoss = new decimal();

            decimal rsi1 = new decimal();

            decimal PrevValue = data[0];

            for (int x = 0; x < data.Count - period; x++)
            {
                if (x == 0)
                {
                    for (int i = 0; i <= period; i++)
                    {
                        if ((data[i] - PrevValue) / PrevValue > 0) AvgGain += Math.Abs(data[i] - PrevValue);
                        else AvgLoss += Math.Abs(data[i] - PrevValue);
                        PrevValue = data[i];
                    }
                }
                else
                {
                    if ((data[x + period] - PrevValue) > 0)
                    {
                        AvgGain = (AvgGain * (period - 1)) + Math.Abs(data[x + period] - PrevValue);
                        AvgLoss = (AvgLoss * (period - 1));
                    }
                    else
                    {
                        AvgGain = (AvgGain * (period - 1));
                        AvgLoss = (AvgLoss * (period - 1)) + Math.Abs(data[x + period] - PrevValue);
                    }
                    PrevValue = data[x+period];
                }
                AvgGain = Math.Abs(AvgGain) / period;
                AvgLoss = Math.Abs(AvgLoss) / period;

                if (AvgGain != 0 && AvgLoss != 0) {
                    Result.Add(100 - (100 / (1 + (AvgGain / AvgLoss))));
                } else if (AvgGain == 0)
                {
                    rsi1 = -100m;
                } else
                {
                    rsi1 = 100m;
                }
            }

            return Result;
        }
    }
}
