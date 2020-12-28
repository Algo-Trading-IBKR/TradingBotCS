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
            ReadAccountUpdate(key);

        }

        public static async Task ReadAccountUpdate(string key, bool allItems = true)
        {

            var filter = new BsonDocument() { { "Type", key } };
            var sort = Builders<BsonDocument>.Sort.Descending("DateTime");
            dynamic doc;
            if (allItems == true)
            {
                doc = await Collection.Find(filter).Sort(sort).ToListAsync();
                foreach(var d in doc)
                {
                    Console.WriteLine(d);
                }
            }
            else
            {
                doc = await Collection.Find(filter).Limit(1).Sort(sort).SingleAsync();
                Console.WriteLine(doc);
            }
            
               
        }

    }
}
