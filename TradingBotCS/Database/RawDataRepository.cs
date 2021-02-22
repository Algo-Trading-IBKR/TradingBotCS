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
    public static class RawDataRepository
    {
        private static readonly string Name = "RawDataRepository";
        private static readonly IMongoDatabase Db = Program.MongoDBClient.GetDatabase("TradingBot");
        private static readonly IMongoCollection<BsonDocument> Collection = Db.GetCollection<BsonDocument>("RawData");

        public static async Task InsertRawData(int tickerId, long time, double open, double high, double low, double close)
        {
            Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Id == tickerId);

            DateTime Time = Converter.UnixTimeStampToDateTime(time);
            //Console.WriteLine(Time);
            ObjectId id = new ObjectId();
            RawData Data = new RawData(id, SymbolObject.Ticker, Time, open, high, low, close);

            SymbolObject.RawDataList.Add(Data);
            SymbolObject.LastRawData = Data;

            BsonDocument Doc = Data.ToBsonDocument();

            await Collection.InsertOneAsync(Doc);

            await SymbolObject.ExecuteStrategy();
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
