﻿using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.IBApi_OverRide
{
    public class WrapperOverride : EWrapperImpl
    {
        private static string Name = "WrapperOverride";
        //! [incrementorderid]
        public void IncrementOrderId()
        {
            NextOrderId++;
        }
        //! [incrementorderid]

        //! [contractdetails]
        public override async void contractDetails(int reqId, ContractDetails contractDetails)
        {
            // Only for printing results in Console
            //Console.WriteLine("ContractDetails begin. ReqId: " + reqId);
            //printContractMsg(contractDetails.Contract);
            //printContractDetailsMsg(contractDetails);
            //Console.WriteLine("ContractDetails end. ReqId: " + reqId);
            foreach (Symbol S in Program.SymbolObjects)
            {
                if (contractDetails.Contract.Symbol == S.Ticker)
                {
                    S.Contract = contractDetails.Contract;
                    S.ContractDetails = contractDetails;
                }
            }
        }
        //! [contractdetails]

        //! [contractdetailsend]
        public override void contractDetailsEnd(int reqId)
        {
            //Console.WriteLine("ContractDetailsEnd. " + reqId + "\n");
        }
        //! [contractdetailsend]

        //! [commissionreport]
        public virtual void commissionReport(CommissionReport commissionReport)
        {
            CommissionReportOverride commissionReportOverride = new CommissionReportOverride(commissionReport);
            CommissionRepository.InsertReport(commissionReportOverride);
            //Console.WriteLine("CommissionReport. " + commissionReport.ExecId + " - " + commissionReport.Commission + " " + commissionReport.Currency + " RPNL " + commissionReport.RealizedPNL);
            Logger.Info(Name, $"Profit: {commissionReportOverride.RealizedPNL}");
        }
        //! [commissionreport]

        //! [execdetails]
        public virtual void execDetails(int reqId, Contract contract, Execution execution)
        {
            ExecutionOverride executionOverride = new ExecutionOverride(execution);
            ExecutionRepository.InsertReport(contract, executionOverride);
            //Console.WriteLine("ExecDetails. " + reqId + " - " + contract.Symbol + ", " + contract.SecType + ", " + contract.Currency + " - " + execution.ExecId + ", " + execution.OrderId + ", " + execution.Shares + ", " + execution.LastLiquidity);
        }
        //! [execdetails]

        //! [openorder]
        public override async void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            //Console.WriteLine("OpenOrder. PermID: " + order.PermId + ", ClientId: " + order.ClientId + ", OrderId: " + orderId + ", Account: " + order.Account +
            //    ", Symbol: " + contract.Symbol + ", SecType: " + contract.SecType + " , Exchange: " + contract.Exchange + ", Action: " + order.Action + ", OrderType: " + order.OrderType +
            //    ", TotalQty: " + order.TotalQuantity + ", CashQty: " + order.CashQty + ", LmtPrice: " + order.LmtPrice + ", AuxPrice: " + order.AuxPrice + ", Status: " + orderState.Status);
            await OrderManager.CheckOrder(order);
            Console.WriteLine("opentest");
        }
        //! [openorder]

        //! [openorderend]
        public override void openOrderEnd()
        {
            //Console.WriteLine("OpenOrderEnd");
        }
        //! [openorderend]

        //! [position] request with reqPositions
        public override void position(string account, Contract contract, double pos, double avgCost)
        {
            //Console.WriteLine("Position. " + account + " - Symbol: " + contract.Symbol + ", SecType: " + contract.SecType + ", Currency: " + contract.Currency + ", Position: " + pos + ", Avg cost: " + avgCost);
            PositionsRepository.UpsertPositions(account, contract, pos, avgCost);
        }
        //! [position]

        //! [positionend]
        public override void positionEnd()
        {
            //Console.WriteLine("PositionEnd \n");
        }
        //! [positionend]

        //! [realtimebar]
        public override void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            //Console.WriteLine("RealTimeBars. " + reqId + " - Time: " + time + ", Open: " + open + ", High: " + high + ", Low: " + low + ", Close: " + close + ", Volume: " + volume + ", Count: " + count + ", WAP: " + WAP);
            RawDataRepository.InsertRawData(reqId, time, open, high, low, close);// data moet ook toegevoegd worden aan symbol raw data list
        }
        //! [realtimebar]

        //! [updateaccountvalue]
        public override async void updateAccountValue(string key, string value, string currency, string accountName)
        {
            List<string> WantedValues = new List<string>() { "cashbalance","unrealizedpnl","netliquidationbycurrency","pasharesvalue" };
            //Console.WriteLine("UpdateAccountValue. Key: " + key + ", Value: " + value + ", Currency: " + currency + ", AccountName: " + accountName);
            if (WantedValues.Contains(key, StringComparer.OrdinalIgnoreCase) && currency.ToLower() == "usd"){
                AccountRepository.InsertAccountUpdate(key.ToLower(), value, currency.ToLower(), accountName);
            }

            if (key.ToLower().Equals("cashbalance") && currency.ToLower() == "usd")
            {
                Symbol.CashBalance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                Logger.Info(Name, $"Cash: ${value}");
            }else if (key.ToLower().Equals("unrealizedpnl") && currency.ToLower() == "usd")
            {
                Logger.Info(Name, $"Unrealized P&L: ${value}");
            }
        }
        //! [updateaccountvalue]

        //! [updateaccounttime]
        public override void updateAccountTime(string timestamp)
        {
            //Console.WriteLine("UpdateAccountTime. Time: " + timestamp + "\n");
        }
        //! [updateaccounttime]

        //! [updateportfolio]
        public override void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            //Console.WriteLine("UpdatePortfolio. " + contract.Symbol + ", " + contract.SecType + " @ " + contract.Exchange
            // + ": Position: " + position + ", MarketPrice: " + marketPrice + ", MarketValue: " + marketValue + ", AverageCost: " + averageCost
            // + ", UnrealizedPNL: " + unrealizedPNL + ", RealizedPNL: " + realizedPNL + ", AccountName: " + accountName);
        }
        //! [updateportfolio]

        //! [historicalDataUpdate] !!!was not marked as virtual!!!
        public override void historicalDataUpdate(int reqId, Bar bar)
        {
            Console.WriteLine("HistoricalDataUpdate. " + reqId + " - Time: " + bar.Time + ", Open: " + bar.Open + ", High: " + bar.High + ", Low: " + bar.Low + ", Close: " + bar.Close + ", Volume: " + bar.Volume + ", Count: " + bar.Count + ", WAP: " + bar.WAP);
        }
        //! [historicalDataUpdate]

        //! [historicaldataend]
        public override void historicalDataEnd(int reqId, string startDate, string endDate)
        {
            Console.WriteLine("HistoricalDataEnd - " + reqId + " from " + startDate + " to " + endDate);
        }
        //! [historicaldataend]

        //! [scannerparameters]
        public override void scannerParameters(string xml)
        {
            Console.WriteLine("ScannerParameters. " + xml + "\n");
            System.IO.File.WriteAllText(@"C:\Users\Public\WriteLines.txt", xml);
        }
        //! [scannerparameters]

        //! [scannerdata]
        public override void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            Console.WriteLine("ScannerData. " + reqId + " - Rank: " + rank + ", Symbol: " + contractDetails.Contract.Symbol + ", SecType: " + contractDetails.Contract.SecType + ", Currency: " + contractDetails.Contract.Currency
                + ", Distance: " + distance + ", Benchmark: " + benchmark + ", Projection: " + projection + ", Legs String: " + legsStr);

            Program.SymbolList = 

        }
        //! [scannerdata]

        //! [scannerdataend]
        public override void scannerDataEnd(int reqId)
        {
            Console.WriteLine("ScannerDataEnd. " + reqId);
        }
        //! [scannerdataend]
    }
}
