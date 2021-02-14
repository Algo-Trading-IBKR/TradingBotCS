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
using TradingBotCS.Util;

namespace TradingBotCS.Database
{
    public static class ConfigRepository
    {
        private static readonly string Name = "ConfigRepository";
        private static readonly IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static readonly IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("Config");

        public static async Task UpsertConfig(Configuration config)
        {
            //DateTime Time = DateTime.Now;
            //ObjectId Id = new ObjectId();

            //config.DateTime = Time;
            //config.Id = Id;

            //BsonDocument Doc = config.ToBsonDocument();

            //await Collection.InsertOneAsync(Doc);

            //#####################################################
            BsonDocument doc;

            var Filter = new BsonDocument() { { "Name", "Main" } };

            try
            {
                doc = await Collection.Find(Filter).Limit(1).SingleAsync();
            }
            catch
            {
                DateTime Time = DateTime.Now;
                config.DateTime = Time;
                BsonDocument Doc = config.ToBsonDocument();

                await Collection.InsertOneAsync(Doc);
                doc = Doc;
            }

            var IdFilter = Builders<BsonDocument>.Filter.Eq("Name", (string)doc.GetElement("Name").Value);

            var Update = Builders<BsonDocument>.Update.Set("DateTime", DateTime.Now).Set("ApiConfig", config.ApiConfig).Set("StrategyConfig", config.StrategyConfig).Set("BuyConfig", config.BuyConfig).Set("SellConfig", config.SellConfig).Set("ErrorCodeConfig", config.ErrorCodeConfig).Set("MessagingConfig", config.MessagingConfig);

            var Options = new UpdateOptions { IsUpsert = true };

            await Collection.UpdateOneAsync(IdFilter, Update, Options);
        }

        public static async Task<Configuration> ReadConfig()
        {
            
            var Filter = new BsonDocument() { { "Name", "Main" } };
            dynamic result;
            Configuration Doc;

            result = await Collection.Find(Filter).Limit(1).SingleAsync();
            Doc = BsonSerializer.Deserialize<Configuration>(result);
            return Doc;
        }

    }
}
