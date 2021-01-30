using System;
using System.Collections.Generic;
using System.Text;

namespace DataCollectorCS
{
    public class HistoricalRequest
    {

        public int Id { get; set; }

        public DateTime DateTime { get; set; }


        public HistoricalRequest(int id)
        {
            this.Id = id;
            this.DateTime = DateTime.Now;
        }

        public override string ToString()
        {
            return Id + ": " + DateTime;
        }

    }
}
