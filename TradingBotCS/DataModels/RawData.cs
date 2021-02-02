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
        public ObjectId _id { get; set; }
        public string Symbol { get; set; }
        public DateTime DateTime { get; set; }
        public double Open { get; set; }
        public double High { get; set; }
        public double Low { get; set; }
        public double Close { get; set; }
        public double Volume { get; set; }

        public RawData(ObjectId id, string symbol, DateTime time, double open, double high, double low, double close, double volume = 0)
        {
            this._id = id;
            this.Symbol = symbol;
            this.DateTime = time;
            this.Open = open;
            this.High = high;
            this.Low = low;
            this.Close = close;
            this.Volume = volume;
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
