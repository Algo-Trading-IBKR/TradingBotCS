using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS
{
    public static class OrderManager
    {
        // zal waarschijnlijk veranderen en gebruik maken van database, dit is maar een placeholder
        public static async Task CheckOrder(Order order)
        {
            foreach(Symbol S in Program.SymbolObjects)
            {
                if(S.LatestOrder.OrderId == order.OrderId)
                {
                    if(S.LatestOrder.Action == "BUY" && order.Action == "BUY")
                    {
                        if((float)S.LastRawData.Close > (S.AvgPrice*1.005))
                        {
                            Program.IbClient.ClientSocket.cancelOrder(order.OrderId);
                        }
                    }
                    else if(S.LatestOrder.Action == "SELL" && order.Action == "SELL")
                    {
                        if ((float)S.LastRawData.Close < (S.AvgPrice * 0.995))
                        {
                            Program.IbClient.ClientSocket.cancelOrder(order.OrderId);
                        }
                    }
                }
            }
            Console.WriteLine("checktest");
        }
    }
}
