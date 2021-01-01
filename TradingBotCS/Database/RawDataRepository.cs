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
using TradingBotCS.HelperClasses;

namespace TradingBotCS.Database
{
    public static class RawDataRepository
    {
        private static string Name = "RawDataRepository";
        private static IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("RawData");

        public static async Task InsertRawData(int tickerId, long time, double open, double high, double low, double close)
        {
            Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Id == tickerId);
            DateTime Time = Converter.UnixTimeStampToDateTime(time);
            ObjectId id = new ObjectId();
            RawData Data = new RawData(id, SymbolObject.Ticker, Time, open, high, low, close);
            BsonDocument Doc = Data.ToBsonDocument();

            await Collection.InsertOneAsync(Doc);
            //List<RawData> test =  await ReadRawData("AMD");
        }

        public static async Task<List<RawData>> ReadRawData(string symbol, int amount=5000)
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
