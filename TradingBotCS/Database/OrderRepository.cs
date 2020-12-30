using IBApi;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.IBApi_OverRide;

namespace TradingBotCS.Database
{
    class OrderRepository
    {
        private static string Name = "OrderRepository";
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("OrderLog");

        public static async Task InsertReport(OrderOverride order)
        {
            BsonDocument Doc = order.ToBsonDocument();
            await Collection.InsertOneAsync(Doc);
        }

        public static async Task<OrderOverride> GetOrderById(string symbol, int orderId)
        {
            var Filter = new BsonDocument() { { "Symbol", symbol }, { "OrderId", orderId } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");

            BsonDocument Doc = await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync();
            OrderOverride order = BsonSerializer.Deserialize<OrderOverride>(Doc);

            return order;
        }
    }
}
