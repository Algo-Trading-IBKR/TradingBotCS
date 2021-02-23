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
    class ExecutionRepository
    {
        private static readonly string Name = "ExecutionReport";
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("ExecutionLog");

        
        public static async Task InsertReport(Contract contract, ExecutionOverride executionOverride)
        {
            OrderOverride order = await OrderRepository.GetOrderById(contract.Symbol, executionOverride.OrderId);

            executionOverride.Order = order;

            BsonDocument Doc = executionOverride.ToBsonDocument();
            await Collection.InsertOneAsync(Doc);
        }

        public static async Task<ExecutionOverride> GetExecutionById(string ExecId)
        {
            var Filter = new BsonDocument() { { "ExecId", ExecId } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");

            BsonDocument Doc = await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync();
            ExecutionOverride Execution = BsonSerializer.Deserialize<ExecutionOverride>(Doc);

            return Execution;
        }

    }
}
