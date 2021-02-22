using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.Util;

namespace TradingBotCS.Database
{
    class StrategyDataRepository
    {
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("StrategyData");


        public static async Task InsertRawData(StrategyData data)
        {

            BsonDocument Doc = data.ToBsonDocument();

            await Collection.InsertOneAsync(Doc);

        }

        public static async Task<List<RawData>> ReadRawData(string symbol, int amount = 5000)
        {
            List<RawData> Data = new List<RawData>();

            var Filter = new BsonDocument() { { "Symbol", symbol } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");
            dynamic Doc;
            Doc = await Collection.Find(Filter).Limit(amount).Sort(Sort).ToListAsync();
            foreach (BsonDocument d in Doc)
            {
                RawData Datapoint = BsonSerializer.Deserialize<RawData>(d);
                //Console.WriteLine(Datapoint);
                Data.Insert(0, Datapoint); // omdraaien van list omdat het anders in foute volgorde staat, zie sort
            }
            return Data;
        }
    }
}
