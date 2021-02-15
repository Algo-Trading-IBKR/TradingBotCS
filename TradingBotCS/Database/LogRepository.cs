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
    public class LogRepository
    {
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("Logs");

        public static async Task InsertLog(string name, string error, string type)
        {
            Log Data = new Log(name, error, type);

            BsonDocument Doc = Data.ToBsonDocument();
            await Collection.InsertOneAsync(Doc);
        }


    }
}
