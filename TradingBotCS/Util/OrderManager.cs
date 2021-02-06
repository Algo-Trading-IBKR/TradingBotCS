using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Util;
using TradingBotCS.IBApi_OverRide;

namespace TradingBotCS
{
    public static class OrderManager
    {
        private static string Name = "OrderManager";

        public static async Task<OrderOverride> CreateOrder(string action = "SELL", string type = "LMT", double amount = 0, double price = 1, double trailStopPrice = 0, float priceOffset = 0, double trailingPercent = 0, string tif = "GTC")
        {
            OrderOverride order;
            
            Program.IbClient.IncrementOrderId();
            Program.IbClient.ClientSocket.reqIds(-1);
            Console.WriteLine(Program.IbClient.NextOrderId);
            switch (type)
            {
                case "MKT":
                    Console.WriteLine("Case 1");
                    order = await CreateMKT(action, amount);
                    break;
                case "LMT":
                    Console.WriteLine("Case 2");
                    order = await CreateLMT(action, amount, price);
                    break;
                case "TRAIL LIMIT":
                    Console.WriteLine("Case 3");
                    order = await CreateTrailingStopLimit(action, amount, trailStopPrice, priceOffset, trailingPercent);
                    break;
                default:
                    Logger.Error(Name, $"Order Type {type} Not Allowed");
                    order = new OrderOverride();
                    break;
            }
            // same for all orders
            order.Tif = tif; // Time In Force, how long an order stays active, GTC stays for 3 months, DAY stays till the end of the day

            return order;
        }

        public static async Task<OrderOverride> CreateMKT(string action, double amount)
        {
            OrderOverride order = new OrderOverride();
            order.Action = action;
            order.OrderType = "MKT";
            order.TotalQuantity = Math.Round(amount, 1);
            return order;
        }

        public static async Task<OrderOverride> CreateLMT(string action, double amount, double price)
        {
            OrderOverride order = new OrderOverride();
            order.Action = action;
            order.OrderType = "LMT";
            order.LmtPrice = Math.Round(price, 2);
            order.TotalQuantity = Math.Round(amount, 1);
            return order;
        }

        //"SELL", "TRAIL LIMIT", position, averageCost*1.04, PriceOffset, trailingpercent

        public static async Task<OrderOverride> CreateTrailingStopLimit(string action, double amount, double trailStopPrice, float priceOffset, double trailingPercent)
        {
            OrderOverride order = new OrderOverride();
            order.Action = action;
            order.OrderType = "TRAIL LIMIT";
            order.TotalQuantity = Math.Round(amount, 1);
            order.TrailStopPrice = Math.Round(trailStopPrice, 2);
            order.LmtPriceOffset = Math.Round(priceOffset, 2);
            order.TrailingPercent = Math.Round(trailingPercent, 2);
            
            return order;
        }

        
    }
}
