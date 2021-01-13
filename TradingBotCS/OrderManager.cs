using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.HelperClasses;

namespace TradingBotCS
{
    public static class OrderManager
    {
        private static string Name = "OrderManager";
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

        public static async Task<Order> CreateMKTOrder(string action, int amount)
        {
            Order order = new Order();
            order.Action = action;
            order.OrderType = "MKT";
            order.TotalQuantity = amount;
            return order;
        }

        //"SELL", "TRAIL LIMIT", position, averageCost*1.04, PriceOffset, trailingpercent
    
        public static async Task<Order> CreateTrailingStopLimit(string action, double amount, double trailStopPrice, float priceOffset, int trailingPercent)
        {
            Order order = new Order();
            order.Action = action;
            order.OrderType = "TRAIL LIMIT";
            order.TotalQuantity = amount;
            order.TrailStopPrice = trailStopPrice;
            order.LmtPriceOffset = priceOffset;
            order.TrailingPercent = trailingPercent;
            
            return order;
        }

        public static async Task<Order> CreateOrder(string action, string type = "LMT", int amount = 0, double trailStopPrice = 0, float priceOffset = 0, int trailingPercent = 0)
        {
            Order order;
            switch (type)
            {
                case "MKT":
                    Console.WriteLine("Case 1");
                    order = await CreateMKTOrder(action, amount);
                    return order;
                case "LMT":
                    Console.WriteLine("Case 2");
                    break;
                case "TRAIL LIMIT":
                    Console.WriteLine("Case 3");
                    order = await CreateTrailingStopLimit(action, amount, trailStopPrice, priceOffset, trailingPercent);
                    return order;
                default:
                    Logger.Critical(Name, "Order Type Not Allowed");
                    break;
            }
        }

    }
}
