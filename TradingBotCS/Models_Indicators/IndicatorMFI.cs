using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Models_Indicators
{
    public static class IndicatorMFI
    {
        // Money flow indicator
        public static async Task<decimal> MFI(List<decimal> low, List<decimal> high,List<decimal> open, List<decimal> close, List<decimal> volume, int period)
        {
            List<decimal> TypicalPrices = await CalculateTypicalPrices(low, high, close);
            List<decimal> PositiveMF = new List<decimal>(); 
            List<decimal> NegativeMF = new List<decimal>(); 

        for (int i = 1; i <= period; i++) {
            decimal PrevTypicalPrice = data[i-1];
            decimal CurrentTypicalPrice = data[i];

            if(CurrentTypicalPrice > PrevTypicalPrice){
                PositiveMF.Add(CurrentTypicalPrice*volume[i]);
            } else {
                NegativeMF.Add(CurrentTypicalPrice*volume[i]);
            }
        }
        decimal PositiveSum;
        foreach (decimal price in PositiveMF) {PositiveSum += price;}
        decimal NegativeSum;
        foreach (decimal price in NegativeMF) {NegativeSum += price;}

        decimal MoneyRatio = PositiveSum/NegativeSum;

        decimal MoneyFlow = 100 = (100 / (1 + MoneyRatio));

        return MoneyFlow;
        }
        public static async Task<List<decimal>> CalculateTypicalPrices(List<decimal> low, List<decimal> high, List<decimal> close)
        {
            List<decimal> TypicalPrices = new List<decimal>();
            for(int i = 0; i < low.Count; i++){
                TypicalPrices.Add((low[i] + high[i] + close[i]) / 3);
            }

            return TypicalPrices;
        }
        
    }



}
