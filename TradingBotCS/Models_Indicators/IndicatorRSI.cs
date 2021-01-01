﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Models_Indicators
{
    public static class IndicatorRSI
    {
        public static async Task<List<decimal>> RSI(List<decimal> data, int period)
        {
            List<decimal> Result = new List<decimal>();
            decimal AvgGain = new decimal();
            decimal AvgLoss = new decimal();

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
                    PrevValue = data[x + period];
                }
                AvgGain = Math.Abs(AvgGain) / period;
                AvgLoss = Math.Abs(AvgLoss) / period;

                if (AvgGain != 0 && AvgLoss != 0)
                {
                    Result.Add(100 - (100 / (1 + (AvgGain / AvgLoss))));
                }
            }
            return Result;
        }

        public static async Task<List<decimal>> RSI(List<int> data, int period)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await RSI(DecList, period);
            return Result;
        }
        public static async Task<List<decimal>> RSI(List<float> data, int period)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await RSI(DecList, period);
            return Result;
        }
        public static async Task<List<decimal>> RSI(List<double> data, int period)
        {
            List<decimal> DecList = new List<decimal>();
            data.ForEach(item => DecList.Add((decimal)item));
            List<decimal> Result = await RSI(DecList, period);
            return Result;
        }


        public static async Task<(List<decimal>,List<decimal>)> stochRSI(List<decimal> data, int Kperiod, int Dperiod)
        {
            //Fast %K =  ( ( Close - rsi(Low) ) / (rsi( High) - rsi(Low )) )

            List<decimal> K = new List<decimal>();
            List<decimal> D = new List<decimal>();

            for (int i = 0; i < data.Count-Kperiod; i++)
            {
                List<decimal> ShortList = data.GetRange(i, Kperiod);
                decimal Low = ShortList.Min(); //laagste van de lijst :p
                decimal High = ShortList.Max(); //hoogste van de lijst >:(
                //Console.WriteLine($"Low: {Low}");
                //Console.WriteLine($"high: {High}");
                if (High - Low == 0) // geen idee of dees een goe idee is
                {
                    decimal fastk = K.Last();
                    K.Add(fastk);
                    //Console.WriteLine($"FastK: {fastk}");
                }
                else {
                    decimal fastk = (ShortList[0] - Low) / (High - Low);
                    K.Add(fastk * 100);
                    //Console.WriteLine($"FastK: {fastk*100}");
                }                
            }
            D = await IndicatorMA.SMA(K, Dperiod);


            //for (int i = 0; i < ResultK.Count; i++)
            //{
            //    Console.WriteLine(ResultK[i]);
            //    Console.WriteLine(ResultD[i]);
            //}

            return (K, D);
        }

    }
}
