using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.HelperClasses;
using TradingBotCS.IBApi_OverRide;

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

        public static async Task<OrderOverride> CreateOrder(string action, string type = "LMT", double amount = 0, double trailStopPrice = 0, float priceOffset = 0, double trailingPercent = 0)
        {
            OrderOverride order;
            Program.IbClient.IncrementOrderId();
            switch (type)
            {
                case "MKT":
                    Console.WriteLine("Case 1");
                    order = await CreateMKT(action, amount);
                    return order;
                case "LMT":
                    Console.WriteLine("Case 2");
                    order = new OrderOverride();
                    return order;
                case "TRAIL LIMIT":
                    Console.WriteLine("Case 3");
                    order = await CreateTrailingStopLimit(action, amount, trailStopPrice, priceOffset, trailingPercent);
                    return order;
                default:
                    Logger.Critical(Name, $"Order Type {type} Not Allowed");
                    order = new OrderOverride();
                    return order;
            }
        }

        public static async Task<OrderOverride> CreateMKT(string action, double amount)
        {
            OrderOverride order = new OrderOverride();
            order.Action = action;
            order.OrderType = "MKT";
            order.TotalQuantity = amount;
            return order;
        }

        public static async Task<OrderOverride> CreateLMT(string action, double amount, float price)
        {
            OrderOverride order = new OrderOverride();
            order.Action = action;
            order.OrderType = "LMT";
            order.LmtPrice = price;
            order.TotalQuantity = amount;
            return order;
        }

        //"SELL", "TRAIL LIMIT", position, averageCost*1.04, PriceOffset, trailingpercent

        public static async Task<OrderOverride> CreateTrailingStopLimit(string action, double amount, double trailStopPrice, float priceOffset, double trailingPercent)
        {
            OrderOverride order = new OrderOverride();
            order.Action = action;
            order.OrderType = "TRAIL LIMIT";
            order.TotalQuantity = amount;
            //order.TrailStopPrice = trailStopPrice;
            order.LmtPriceOffset = priceOffset;
            order.TrailingPercent = trailingPercent;
            //order.AuxPrice = 0.1;
            
            return order;
        }

        
    }
}
