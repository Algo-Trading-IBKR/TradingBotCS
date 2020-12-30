using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    [BsonIgnoreExtraElements]
    public class RawData
    {
        //[BsonId]
        public ObjectId _id { get; set; }

        //[BsonElement("Symbol")]
        public string Symbol { get; set; }

        //[BsonElement("DateTime")]
        public DateTime DateTime { get; set; }

        //[BsonElement("Open")]
        public double Open { get; set; }

        //[BsonElement("High")]
        public double High { get; set; }

        //[BsonElement("Low")]
        public double Low { get; set; }

        //[BsonElement("Close")]
        public double Close { get; set; }

        public RawData(ObjectId id, string symbol, DateTime time, double open, double high, double low, double close)
        {
            this._id = id;
            this.Symbol = symbol;
            this.DateTime = time;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            
        }
        public RawData()
        {

        }
        public override string ToString()
        {
            return Symbol + ": " + Close;
        }
    }
}
