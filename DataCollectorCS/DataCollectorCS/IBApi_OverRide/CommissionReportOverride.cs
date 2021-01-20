﻿using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.IBApi_OverRide
{
    public class CommissionReportOverride : CommissionReport
    {
        private static string Name = "CommissionReportOverride";

        public DateTime DateTime { get; set; }

        public CommissionReportOverride(CommissionReport commissionReport)
        {
            try
            {
                DateTime = DateTime.Now;
                ExecId = commissionReport.ExecId;
                Currency = commissionReport.Currency;
                Commission = commissionReport.Commission;
                RealizedPNL = commissionReport.RealizedPNL;
                Yield = commissionReport.Yield;
                YieldRedemptionDate = commissionReport.YieldRedemptionDate;
            }
            catch (Exception ex)
            {
                Logger.Error(Name, $"{ex}");
            }
        }
    }
}
