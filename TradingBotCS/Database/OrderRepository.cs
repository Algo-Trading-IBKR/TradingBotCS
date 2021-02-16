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
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("Orders");

        public static async Task InsertOrder(OrderOverride order)
        {
            BsonDocument Doc = order.ToBsonDocument();
            await Collection.InsertOneAsync(Doc);
        }

        public static async Task UpsertOrder(OrderOverride order)
        {
            BsonDocument Doc;
            var Filter = new BsonDocument(){ { "Contract.Symbol", order.Contract.Symbol } };
            try
            {
                Doc = await Collection.Find(Filter).Limit(1).SingleAsync();
            }
            catch
            {
                Doc = order.ToBsonDocument();
                await Collection.InsertOneAsync(Doc);
            }
            
        }

        public static async Task<OrderOverride> GetOrderById(string symbol, int orderId)
        {
            var Filter = new BsonDocument() { { "Contract.Symbol", symbol }, { "OrderId", orderId } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");

            BsonDocument Doc = await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync();
            OrderOverride order = BsonSerializer.Deserialize<OrderOverride>(Doc);

            return order;
        }

        public static async Task DeleteOrders(string symbol)
        {
            var Filter = new BsonDocument() { { "Contract.Symbol", symbol } };
            try
            {
                for(int i = 0; i < 5; i++){
                     await Collection.FindOneAndDeleteAsync(Filter); 
                }
            }
            catch (Exception)
            { }
        }

        public static async Task<List<OrderOverride>> GetAllOrders()
        {
            var Filter = new BsonDocument();
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");
            List<OrderOverride> Orders = new List<OrderOverride>();

            List<BsonDocument> Doc = await Collection.Find(Filter).Sort(Sort).ToListAsync();
            foreach(BsonDocument D in Doc)
            {
                Orders.Add(BsonSerializer.Deserialize<OrderOverride>(D));
            }
            
            return Orders;
        }
    }
}
