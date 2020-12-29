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
    public static class AccountRepository
    {
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("AccountInfo");

        public static async Task InsertAccountUpdate(string key, string value, string currency, string accountName)
        {
            BsonDocument Doc = new BsonDocument
            {
                {"AccountId", accountName},
                {"DateTime", DateTime.Now},
                {"Type", key },
                {"Value", value }
            };

            await Collection.InsertOneAsync(Doc);
            //ReadAccountUpdate(key);

        }

        public static async Task ReadAccountUpdate(string key, bool allItems = true)
        {
            var Filter = new BsonDocument() { { "Type", key } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");
            dynamic Doc;
            if (allItems == true)
            {
                Doc = await Collection.Find(Filter).Sort(Sort).ToListAsync();
                foreach(var d in Doc)
                {
                    Console.WriteLine(d);
                }
            }
            else
            {
                Doc = await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync();
                Console.WriteLine(Doc);
            }
        }

    }
}
