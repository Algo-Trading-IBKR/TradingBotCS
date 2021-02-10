using IBApi;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;
using TradingBotCS.Util;

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

        //! [nextvalidid]
        public override void nextValidId(int orderId)
        {
            Console.WriteLine("Next Valid Id: " + orderId);
            NextOrderId = orderId;
        }
        //! [nextvalidid]

        //! [error]
        public override void error(int id, int errorCode, string errorMsg)
        {
            //Console.WriteLine("Error. Id: " + id + ", Code: " + errorCode + ", Msg: " + errorMsg + "\n");
            Logger.Warn(Name, $"Error. Id: {id}, Code: {errorCode} Msg: {errorMsg} \n");
        }
        //! [error]

        public override void error(Exception e)
        {
            //Console.WriteLine("Exception thrown: " + e);
            Logger.Error(Name, $"Exception thrown: {e}");
            throw e;
        }

        public override void error(string str)
        {
            //Console.WriteLine("Error: " + str + "\n");
            Logger.Error(Name, $"Error: {str}");
        }

        //! [contractdetails]
        public override async void contractDetails(int reqId, ContractDetails contractDetails)
        {
            // Only for printing results in Console
            //Console.WriteLine("ContractDetails begin. ReqId: " + reqId);
            //printContractMsg(contractDetails.Contract);
            //printContractDetailsMsg(contractDetails);
            //Console.WriteLine("ContractDetails end. ReqId: " + reqId);
            if (reqId == 99999)
            {
                Program.TestSymbol.Contract = contractDetails.Contract;
                Program.TestSymbol.ContractDetails = contractDetails;
            }
            else
            {
                foreach (Symbol S in Program.SymbolObjects)
                {
                    if (contractDetails.Contract.Symbol == S.Ticker)
                    {
                        S.Contract = contractDetails.Contract;
                        S.ContractDetails = contractDetails;
                    }
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
        public override void commissionReport(CommissionReport commissionReport)
        {
            CommissionReportOverride commissionReportOverride = new CommissionReportOverride(commissionReport);
            CommissionRepository.InsertReport(commissionReportOverride);
            Console.WriteLine("CommissionReport. " + commissionReport.ExecId + " - " + commissionReport.Commission + " " + commissionReport.Currency + " RPNL " + commissionReport.RealizedPNL);
            if (commissionReport.RealizedPNL != 0)
            {
                Logger.Info(Name, $"Profit: {commissionReportOverride.RealizedPNL}");
            }

        }
        //! [commissionreport]

        //! [execdetails]
        public override void execDetails(int reqId, Contract contract, Execution execution)
        {
            ExecutionOverride executionOverride = new ExecutionOverride(execution);
            ExecutionRepository.InsertReport(contract, executionOverride);
            Console.WriteLine("ExecDetails. " + reqId + " - " + contract.Symbol + ", " + contract.SecType + ", " + contract.Currency + " - " + execution.ExecId + ", " + execution.OrderId + ", " + execution.Shares + ", " + execution.LastLiquidity);
        }
        //! [execdetails]

        //! [openorder]
        public override async void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            // Console.WriteLine("OpenOrder. PermID: " + order.PermId + ", ClientId: " + order.ClientId + ", OrderId: " + orderId + ", Account: " + order.Account +
            //    ", Symbol: " + contract.Symbol + ", SecType: " + contract.SecType + " , Exchange: " + contract.Exchange + ", Action: " + order.Action + ", OrderType: " + order.OrderType +
            //    ", TotalQty: " + order.TotalQuantity + ", CashQty: " + order.CashQty + ", LmtPrice: " + order.LmtPrice + ", AuxPrice: " + order.AuxPrice + ", Status: " + orderState.Status);
            //await OrderManager.CheckOrder(order);
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
            // Console.WriteLine("Position. " + account + " - Symbol: " + contract.Symbol + ", SecType: " + contract.SecType + ", Currency: " + contract.Currency + ", Position: " + pos + ", Avg cost: " + avgCost);
            PositionsRepository.UpsertPositions(account, contract, pos, avgCost);
        }
        //! [position]

        //! [updateportfolio]
        public override async void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            //Console.WriteLine("UpdatePortfolio. " + contract.Symbol + ", " + contract.SecType + " @ " + contract.Exchange
            //    + ": Position: " + position + ", MarketPrice: " + marketPrice + ", MarketValue: " + marketValue + ", AverageCost: " + averageCost
            //    + ", UnrealizedPNL: " + unrealizedPNL + ", RealizedPNL: " + realizedPNL + ", AccountName: " + accountName);

            // als unrealized > 5% stuur sell order met limit price op die 5%
            if (Program.UseTrailLimitOrders && position > 0 && unrealizedPNL/(averageCost*position) > 0.09)
            {
                Logger.Info(Name, $"{contract.Symbol} unrealized at {unrealizedPNL} - {unrealizedPNL/(position*averageCost)}%");
                float PriceOffset = 0.01f;
                double TrailingPercent = 8;

                OrderOverride Order = await OrderManager.CreateOrder(action: "SELL", type:"TRAIL LIMIT", amount: position, trailStopPrice: marketPrice * (1- (TrailingPercent/100)), priceOffset: PriceOffset, trailingPercent: TrailingPercent);
                //OrderOverride Order = await OrderManager.CreateOrder("SELL", "MKT", position);
                contract = await ContractManager.CreateContract(contract.Symbol);
                
                Program.IbClient.ClientSocket.placeOrder(Program.IbClient.NextOrderId, contract, Order);
                Thread.Sleep(20);
            }
        }
        //! [updateportfolio]

        //! [positionend]
        public override void positionEnd()
        {
            //Console.WriteLine("PositionEnd \n");
        }
        //! [positionend]

        //! [realtimebar]
        public override void realtimeBar(int reqId, long time, double open, double high, double low, double close, long volume, double WAP, int count)
        {
            Console.WriteLine("RealTimeBars. " + reqId + " - Time: " + time + ", Open: " + open + ", High: " + high + ", Low: " + low + ", Close: " + close + ", Volume: " + volume + ", Count: " + count + ", WAP: " + WAP);
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

        //! [historicaldata]
        public override void historicalData(int reqId, Bar bar)
        {
            //Console.WriteLine("HistoricalData. " + reqId + " - Time: " + bar.Time + ", Open: " + bar.Open + ", High: " + bar.High + ", Low: " + bar.Low + ", Close: " + bar.Close + ", Volume: " + bar.Volume + ", Count: " + bar.Count + ", WAP: " + bar.WAP);
            
            //Logger.Info("barTime: " , bar.Time);
            Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Id == reqId);

            string sTime = bar.Time.Insert( 4, "-");
            sTime = sTime.Insert(7, "-");
            
            string[] words = sTime.Split('-');
            string[] morewords = words[2].Split(' ');

            sTime = morewords[0] + "-" + words[1] + "-" + words[0] + " " + morewords[2];

            DateTime Time = Convert.ToDateTime(sTime);
            
            ObjectId id = new ObjectId();

            if (SymbolObject.GapCalculated == true)
            {
                RawData existingData;
                existingData = SymbolObject.HistoricalData.Find(i => i.DateTime == Time);
                if(existingData != null)
                {
                    Logger.Verbose(Name, $"{SymbolObject.Ticker}: datapoint already exists");
                }
                else
                {
                    RawData data = new RawData(id, SymbolObject.Ticker, Time, bar.Open, bar.High, bar.Low, bar.Close);
                    SymbolObject.HistoricalData.Add(data);
                }
                
            }
            else
            {
                RawData data = new RawData(id, SymbolObject.Ticker, Time, bar.Open, bar.High, bar.Low, bar.Close);
                SymbolObject.HistoricalData.Add(data);

            }
        }
        //! [historicaldata]

        //! [historicaldataend]
        public override void historicalDataEnd(int reqId, string startDate, string endDate)
        {
            //Console.WriteLine("HistoricalDataEnd - " + reqId + " from " + startDate + " to " + endDate);
            Program.GettingHistoricalData -= 1;
            Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Id == reqId);
            if (SymbolObject.GapCalculated == false)
            {
                SymbolObject.CalculateGap();
            } else if (SymbolObject.GapCalculated == true)
            {
                SymbolObject.ExecuteStrategy(true);

                //vraag normale market data op voor sell
            }
        }
        //! [historicaldataend]

        //! [scannerparameters]
        public override void scannerParameters(string xml)
        {
            // Console.WriteLine("ScannerParameters. " + xml + "\n");
            System.IO.File.WriteAllText(@"C:\Users\Public\WriteLines.txt", xml);
        }
        //! [scannerparameters]

        //! [scannerdata]
        public override void scannerData(int reqId, int rank, ContractDetails contractDetails, string distance, string benchmark, string projection, string legsStr)
        {
            //Console.WriteLine("ScannerData. " + reqId + " - Rank: " + rank + ", Symbol: " + contractDetails.Contract.Symbol + ", SecType: " + contractDetails.Contract.SecType + ", Currency: " + contractDetails.Contract.Currency
            //+ ", Distance: " + distance + ", Benchmark: " + benchmark + ", Projection: " + projection + ", Legs String: " + legsStr);

            Program.SymbolList.Add(contractDetails.Contract.Symbol);

        }
        //! [scannerdata]

        //! [scannerdataend]
        public override void scannerDataEnd(int reqId)
        {
            //Console.WriteLine("ScannerDataEnd. " + reqId);
            Program.IbClient.ClientSocket.cancelScannerSubscription(reqId);
            Program.ScannerReady = true;
        }
        //! [scannerdataend]
    }
}
