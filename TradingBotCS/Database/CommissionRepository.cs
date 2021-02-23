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
    class CommissionRepository
    {
        private static string Name = "CommissionRepository";
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("CommissionLog");

        public static async Task InsertReport(CommissionReportOverride commissionReport)
        {
            commissionReport.Execution = await ExecutionRepository.GetExecutionById(commissionReport.ExecId);

            BsonDocument Doc = commissionReport.ToBsonDocument();
            await Collection.InsertOneAsync(Doc);
        }

        public static async Task<List<CommissionReportOverride>> ReadCommissionReports(string symbol, int amount = 5000)
        {
            List<CommissionReportOverride> Data = new List<CommissionReportOverride>();

            var Filter = new BsonDocument() { { "Execution.Order.Contract.Symbol", symbol } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");
            dynamic Doc;
            Doc = await Collection.Find(Filter).Limit(amount).Sort(Sort).ToListAsync();
            foreach (BsonDocument d in Doc)
            {
                CommissionReportOverride Datapoint = BsonSerializer.Deserialize<CommissionReportOverride>(d);

                Data.Add(Datapoint);
            }
            return Data;
        }
    }
}
