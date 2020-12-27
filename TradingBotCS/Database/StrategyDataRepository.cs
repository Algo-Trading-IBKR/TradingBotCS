using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Database
{
    class StrategyDataRepository
    {
        private static IMongoDatabase db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> collection = db.GetCollection<BsonDocument>("AccountInfo");
    }
}
