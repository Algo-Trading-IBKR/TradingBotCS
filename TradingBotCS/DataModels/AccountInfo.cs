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
    public class AccountInfo
    {
        //[BsonId]
        public ObjectId _id { get; set; }

        //[BsonElement("AccountId")]
        public string AccountId { get; set; }

        //[BsonElement("DateTime")]
        public DateTime DateTime { get; set; }

        //[BsonElement("Type")]
        public string Type { get; set; }

        //[BsonElement("Value")]
        public float Value { get; set; }

        public AccountInfo(ObjectId id, string accountId, DateTime datetime, string type, float value)
        {
            this._id = id;
            this.AccountId = accountId;
            this.DateTime = datetime;
            this.Type = type;
            this.Value = value;
        }

        public override string ToString()
        {
            return Type + ": " + Value;
        }
    }
}
