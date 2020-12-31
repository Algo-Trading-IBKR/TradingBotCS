using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.Strategies
{
    public class B_StochFRSI_MACD_S_TrailingPercent
    {
        private static string Name = "B_StochFRSI_MACD_S_TrailingPercent";

        private float TakeProfit { get; set; }
        private float BottomProfit { get; set; }
        private bool PassedBottom { get; set; }

        public async Task<bool> BuyStrategy(float latestPrice, List<StrategyData> data)
        {
            try
            {

                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
                throw;
            }            
        }

        public async Task<bool> SellStrategy(int position, float avgPrice, float latestPrice, List<StrategyData> data, string symbol)
        {
            try
            {
                if (((latestPrice - avgPrice)/avgPrice) > TakeProfit)
                {
                    PassedBottom = true;
                }

                if (((latestPrice - avgPrice) / avgPrice) > TakeProfit*0.01)
                {
                    TakeProfit = ((latestPrice - avgPrice) / avgPrice) - 0.01f;

                    if (((latestPrice - avgPrice) / avgPrice) > 1)
                    {
                        TakeProfit = ((latestPrice - avgPrice) / avgPrice) - ((latestPrice - avgPrice) / avgPrice) * 0.01f;
                    }
                    BottomProfit = TakeProfit - 0.01f;
                    if (TakeProfit > 1)
                    {
                        BottomProfit = TakeProfit * 0.9f;
                    }
                    Logger.Info(Name, $"{symbol} Borders: {BottomProfit}% - {TakeProfit}%");
                }
                else if (((latestPrice - avgPrice) / avgPrice) < TakeProfit && ((latestPrice - avgPrice) / avgPrice) > BottomProfit && PassedBottom == true)
                {
                    Logger.Info(Name, $"Sell Order: {symbol} Current Price: {latestPrice}");
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.Error(Name, ex.ToString());
                throw;
            }          
        }

        public B_StochFRSI_MACD_S_TrailingPercent()
        {
            PassedBottom = false;
            TakeProfit = 0.05f;
        }

    }
}

//def main_strategy(self):
//        try:
//            n = 14
//            Local_dataframe = pd.DataFrame(columns =['Time', 'Price'])
//            Local_dataframe.set_index('Time', inplace = True)

//            local_dataframe = self._dataframe.append(self._df[-1:])

//            local_dataframe['fastk'], local_dataframe['fastd'] = talib.STOCHRSI(local_dataframe['Price'], timeperiod = n, fastk_period = 3, fastd_period = 3, fastd_matype = 0)
//            local_dataframe['macd'], local_dataframe['macdsignal'], local_dataframe['macdhist'] = talib.MACD(local_dataframe['Price'], fastperiod = 12, slowperiod = 28, signalperiod = 9)
//            i = len(self._dataframe) - 1

//            if self._hasstock == False and self._AccountMoney > self._minimumcash:
//                if (local_dataframe.at[local_dataframe.index[i], 'fastk'] <= 15 or local_dataframe.at[local_dataframe.index[i], 'fastd'] <= 15) and(local_dataframe.at[local_dataframe.index[i], 'macdhist'] < 0):
//                    if self._fallingcounter == 0:
//                        self._counterprice = local_dataframe.at[local_dataframe.index[i], 'Price']
//                        self._first2 = local_dataframe.at[local_dataframe.index[i], 'macdhist']
//                        self._fallingcounter += 1

//                    if self._fallingcounter > 0 and self._fallingcounter <= 2:
//                        if local_dataframe.at[local_dataframe.index[i], 'macdhist'] < self._first2:
//                            self._first2 = local_dataframe.at[local_dataframe.index[i], 'macdhist']
//                            self._fallingcounter += 1

//                    if local_dataframe.at[local_dataframe.index[i], 'macdhist'] > 0:
//                        self._fallingcounter = 0

//                    if self._fallingcounter == 3 and self._counterprice * 1.02 > local_dataframe.at[local_dataframe.index[i], 'Price']:
//                        self._fallingcounter = 0
//                        currentPrice = local_dataframe.at[local_dataframe.index[i], 'Price']
//                        print(f'{self._symbol}: bought')
//                        sharecount = math.floor(self._trademinimum / currentPrice)
//                        if sharecount > 0:
//                            self.Order('BUY', sharecount, currentPrice)
//                            self._passed_bottom = False
//                            self._bottom_TakeProfit = 0.055
//                            self._previous_TakeProfit = 0.05
//                            # self._hasstock = True    
                    
//            elif self._hasstock == True:
//                currentPrice = local_dataframe.at[local_dataframe.index[i],'Price'] 
//                if ((currentPrice - self._avgCost)/self._avgCost) > self._bottom_TakeProfit:
//                    self._passed_bottom = True
                    
//                if ((currentPrice - self._avgCost)/self._avgCost) > self._bottom_TakeProfit + 0.01:
//                    self._bottom_TakeProfit = ((currentPrice - self._avgCost)/self._avgCost) - 0.01
                    
//                    if ((currentPrice - self._avgCost)/self._avgCost) > 1:
//                        self._bottom_TakeProfit = ((currentPrice - self._avgCost)/self._avgCost) - ((currentPrice - self._avgCost)/self._avgCost)*0.01
                    
                        
//                    self._previous_TakeProfit = self._bottom_TakeProfit - 0.01
//                    if self._bottom_TakeProfit > 1:
//                        self._previous_TakeProfit = self._bottom_TakeProfit*0.9
                      
//                    print(f"borders worden gerased naar: {self._previous_TakeProfit*100:.2f}%, {self._bottom_TakeProfit*100:.2f}%") # film it and show me , imma run dis rnhaha
                        
//                # als hij tussen de bodem take profit zit en de vorige (hier verkopen)
//                elif  ((currentPrice - self._avgCost)/self._avgCost) < self._bottom_TakeProfit and ((currentPrice - self._avgCost)/self._avgCost) > self._previous_TakeProfit and self._passed_bottom == True:
//                    print(f'{self._symbol}: sold')
//                    print(f"Sold at {((currentPrice - self._avgCost)/self._avgCost)*100:.2f}%")
//                    self.Order('SELL', self._position, currentPrice)
