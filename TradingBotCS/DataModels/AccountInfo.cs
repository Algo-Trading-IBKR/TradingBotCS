﻿using MongoDB.Bson.Serialization.Attributes;
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
        [BsonId]
        public string ObjectId { get; set; }

        [BsonElement("AccountId")]
        public string AccountId { get; set; }

        [BsonElement("DateTime")]
        public DateTime DateTime { get; set; }

        [BsonElement("Type")]
        public string Type { get; set; }

        [BsonElement("Value")]
        public float Value { get; set; }

        public override string ToString()
        {
            return Type + ": " + Value;
        }
    }
}