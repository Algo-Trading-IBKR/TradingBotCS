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
            BsonDocument Doc = new BsonDocument
            {
                {"DateTime", Time},
                {"Symbol", SymbolObject.Ticker},
                {"Open", open},
                {"High", high},
                {"Low", low},
                {"Close", close},
            };

            await Collection.InsertOneAsync(Doc);
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
                RawData Datapoint = new RawData();
                Datapoint.Symbol = d["Symbol"].AsString;
                Datapoint.DateTime = d["DateTime"].ToUniversalTime();
                Datapoint.Open = d["Open"].AsDouble;
                Datapoint.High = d["High"].AsDouble;
                Datapoint.Low = d["Low"].AsDouble;
                Datapoint.Close = d["Close"].AsDouble;
                Data.Add(Datapoint);
                    
            }
            return Data;
        }

    }
}
