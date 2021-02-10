using IBApi;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class Position
    {
        public ObjectId _id { get; set; }
        public string AccountId { get; set; }
        public DateTime DateTime { get; set; }
        public double Shares { get; set; }
        public double AvgCost { get; set; }
        public Contract Contract { get; set; }

        public Position(ObjectId id, string accountId, DateTime datetime, double shares, double avgCost ,Contract contract)
        {
            this._id = id;
            this.AccountId = accountId;
            this.DateTime = datetime;
            this.Shares = shares;
            this.AvgCost = avgCost;
            this.Contract = contract;
        }

        public override string ToString()
        {
            return Contract.Symbol + ": " + Shares;
        }
    }
}
