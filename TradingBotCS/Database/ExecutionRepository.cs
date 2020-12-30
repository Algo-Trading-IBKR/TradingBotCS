using IBApi;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.IBApi_OverRide;

namespace TradingBotCS.Database
{
    class ExecutionRepository
    {
        private static string Name = "ExecutionReport";
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("ExecutionLog");

        

        public static async Task InsertReport(Contract contract, ExecutionOverride executionOverride)
        {
            Order order = await OrderRepository.GetOrderById(contract.Symbol, executionOverride.OrderId);

            executionOverride.Symbol = contract.Symbol;
            executionOverride.Action = order.Action;
            executionOverride.OrderType = order.OrderType;

            BsonDocument Doc = executionOverride.ToBsonDocument();
            await Collection.InsertOneAsync(Doc);
        }
    }
}
