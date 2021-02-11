using IBApi;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;

namespace TradingBotCS.Database
{
    public class PositionsRepository
    {
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("Positions");

        public static async Task UpsertPositions(Position position)
        {
            BsonDocument doc;
           
            var Filter = new BsonDocument() { { "Contract.Symbol", position.Contract.Symbol } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");

            try
            {
                doc = await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync();
            }
            catch
            {
                BsonDocument Doc = position.ToBsonDocument();

                await Collection.InsertOneAsync(Doc);
                doc = Doc;
                //doc = await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync();
            }

            var IdFilter = Builders<BsonDocument>.Filter.Eq("_id", (ObjectId)doc.GetElement("_id").Value);
            var Update = Builders<BsonDocument>.Update.Set("DateTime", DateTime.Now).Set("AvgCost", position.AvgCost).Set("Shares", position.Shares);
            var Options = new UpdateOptions { IsUpsert = true };

            await Collection.UpdateOneAsync(IdFilter, Update, Options);

        }
    }
}
