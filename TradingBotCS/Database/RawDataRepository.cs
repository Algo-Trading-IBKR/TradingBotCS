using IBApi;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.Database
{
    class RawDataRepository
    {
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("RawData");

        public static async Task InsertRawData(int tickerId, long time, double price)
        {
            Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Id == tickerId);
            DateTime Time = Converter.UnixTimeStampToDateTime(time);
            BsonDocument Doc = new BsonDocument
            {
                {"DateTime", Time},
                {"Symbol", SymbolObject.Ticker},
                {"Price", price}
            };

            await Collection.InsertOneAsync(Doc);
        }




    }
}
