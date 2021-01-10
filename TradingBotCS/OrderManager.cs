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
                }
            }
            Console.WriteLine("checktest");
        }

        public static async Task<Order> CreateOrder(string action, string type, int amount)
        {
            Order order = new Order();
            order.Action = action;
            order.OrderType = "MKT";
            order.TotalQuantity = amount;
            return order;
        }

        //"SELL", "TRAIL LIMIT", position, averageCost*1.04, PriceOffset, trailingpercent
    
        public static async Task<Order> CreateTrailingStopLimit(string action, string type, double amount, double trailStopPrice, float priceOffset, int trailingPercent)
        {
            Order order = new Order();
            order.Action = action;
            order.OrderType = type;
            order.TotalQuantity = amount;
            order.TrailStopPrice = trailStopPrice;
            order.LmtPriceOffset = priceOffset;
            order.TrailingPercent = trailingPercent;
            
            return order;
        }
    }
}
