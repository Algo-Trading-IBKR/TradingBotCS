using IBApi;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.IBApi_OverRide
{
    public class OrderOverride : Order
    {

        public ObjectId _id { get; set; }


    }
}
