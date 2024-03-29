﻿using MongoDB.Bson;
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
    public static class AccountRepository
    {
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("AccountInfo");

        public static async Task InsertAccountUpdate(string key, string value, string currency, string accountName)
        {
            ObjectId id = new ObjectId();
            AccountInfo Data = new AccountInfo(id, accountName, DateTime.Now, key, float.Parse(value, System.Globalization.CultureInfo.InvariantCulture));

            BsonDocument Doc = Data.ToBsonDocument();
            await Collection.InsertOneAsync(Doc);
            //ReadAccountUpdate("cashbalance");
        }

        public static async Task<List<AccountInfo>> ReadAccountUpdate(string key, bool allItems = false)
        {
            var Filter = new BsonDocument() { { "Type", key.ToLower() } };
            var Sort = Builders<BsonDocument>.Sort.Descending("DateTime");
            dynamic results;
            List<AccountInfo> Doc = new List<AccountInfo>();
            if (allItems == true)
            {
                results = await Collection.Find(Filter).Sort(Sort).ToListAsync();
                foreach(var d in results)
                {
                    //AccountInfo doc = BsonSerializer.Deserialize<AccountInfo>(d);
                    //Console.WriteLine(doc.Value);
                    Doc.Add(BsonSerializer.Deserialize<AccountInfo>(d));
                }
            }
            else
            {
                Doc.Add(BsonSerializer.Deserialize<AccountInfo>(await Collection.Find(Filter).Limit(1).Sort(Sort).SingleAsync()));
                
                //AccountInfo doc = BsonSerializer.Deserialize<AccountInfo>(Doc);
                //Console.WriteLine(doc.Value);

            }
            return Doc;
        }

    }
}
