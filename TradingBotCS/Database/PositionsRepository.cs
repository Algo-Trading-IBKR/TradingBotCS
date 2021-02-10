using IBApi;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Database
{
    public class PositionsRepository
    {
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("Positions");

        public static async Task UpsertPositions(string account, Contract contract, double pos, double avgCost)
        {
            BsonDocument doc;
           
            var Filter = new BsonDocument() { { "Symbol", contract.Symbol } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");

            try
            {
                doc = await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync();
            }
            catch
            {
                BsonDocument Doc = new BsonDocument
                {
                    {"AccountId", account},
                    {"DateTime", DateTime.Now},
                    {"Symbol", contract.Symbol},
                    {"Position", pos},
                    {"AverageCost", avgCost }
                };
                await Collection.InsertOneAsync(Doc);
                doc = Doc;
                //doc = await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync();
            }

            var IdFilter = Builders<BsonDocument>.Filter.Eq("_id", (ObjectId)doc.GetElement("_id").Value);
            var Update = Builders<BsonDocument>.Update.Set("DateTime", DateTime.Now).Set("AverageCost", avgCost).Set("Position", pos);
            var Options = new UpdateOptions { IsUpsert = true };

            await Collection.UpdateOneAsync(IdFilter, Update, Options);

        }
    }
}
