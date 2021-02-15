using IBApi;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
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

        public static async Task<List<Position>> ReadPositions(string symbol = "*", bool allItems = false)
        {

            var Filter = new BsonDocument() { { "Contract.Symbol", symbol.ToUpper() } };
            
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");
            dynamic results;
            List<Position> Doc = new List<Position>();
            if (allItems)
            {
                results = await Collection.Find(_ => true).Sort(Sort).ToListAsync();
                foreach (var d in results)
                {
                    //AccountInfo doc = BsonSerializer.Deserialize<AccountInfo>(d);
                    //Console.WriteLine(doc.Value);
                    Doc.Add(BsonSerializer.Deserialize<Position>(d));
                }
            }
            else
            {
                Doc.Add(BsonSerializer.Deserialize<Position>(await Collection.Find(Filter).Limit(1).SingleAsync()));
            }
            return Doc;
        }
    }
}
