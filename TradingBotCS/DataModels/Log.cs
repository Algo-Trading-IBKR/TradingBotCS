using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.DataModels
{
    public class Log
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public string Error { get; set; }
        public string Type { get; set; }

        public Log(string name, string error, string type) 
        {
            this.Id = new ObjectId();
            this.Name = name;
            this.Error = error;
            this.Type = type;
        }
    }
}
