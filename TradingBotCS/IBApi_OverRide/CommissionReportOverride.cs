using IBApi;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.IBApi_OverRide
{
    public class CommissionReportOverride : CommissionReport
    {
        public ObjectId _id { get; set; }

        public DateTime DateTime { get; set; }

        public CommissionReportOverride(CommissionReport commissionReport)
        {
            _id = new ObjectId();
            DateTime = DateTime.Now;
            ExecId = commissionReport.ExecId;
            Currency = commissionReport.Currency;
            Commission = commissionReport.Commission;
            RealizedPNL = commissionReport.RealizedPNL;
            Yield = commissionReport.Yield;
            YieldRedemptionDate = commissionReport.YieldRedemptionDate;
        }
    }
}
