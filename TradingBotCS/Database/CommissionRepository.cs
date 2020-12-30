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



            BsonDocument Doc = commissionReport.ToBsonDocument();
            await Collection.InsertOneAsync(Doc);
        }

        public static async Task<List<CommissionReportOverride>> ReadCommissionReports(string symbol, int amount = 5000)
        {
            List<CommissionReportOverride> Data = new List<CommissionReportOverride>();

            var Filter = new BsonDocument() { { "Symbol", symbol } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");
            dynamic Doc;
            Doc = await Collection.Find(Filter).Limit(amount).Sort(Sort).ToListAsync();
            foreach (BsonDocument d in Doc)
            {
                //RawData Datapoint = new RawData();
                //Datapoint.Symbol = d["Symbol"].AsString;
                //Datapoint.DateTime = d["DateTime"].ToUniversalTime();
                //Datapoint.Open = d["Open"].AsDouble;
                //Datapoint.High = d["High"].AsDouble;
                //Datapoint.Low = d["Low"].AsDouble;
                //Datapoint.Close = d["Close"].AsDouble;

                CommissionReportOverride Datapoint = BsonSerializer.Deserialize<CommissionReportOverride>(d);

                Data.Add(Datapoint);

            }
            return Data;
        }
    }
}
