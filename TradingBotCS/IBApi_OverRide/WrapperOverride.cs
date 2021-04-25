using IBApi;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
        private static readonly string Name = "WrapperOverride";

        // prevent P&L to be logged evert 3 minutes
        private static DateTime LastPnL;
        private static readonly int PnLTimeout = 30;
        
        
        //! [incrementorderid]
        public void IncrementOrderId()
        {
            NextOrderId++;
        }
        //! [incrementorderid]

        //! [nextvalidid]
        public override void nextValidId(int orderId)
        {
            //Console.WriteLine("Next Valid Id: " + orderId);
            NextOrderId = orderId;
        }
        //! [nextvalidid]

        //! [error]
        public override void error(int id, int errorCode, string errorMsg)
        {
            //Console.WriteLine("Error. Id: " + id + ", Code: " + errorCode + ", Msg: " + errorMsg + "\n");
            if (Program.InfoCodes.Contains(errorCode))
            {
                Logger.Info(Name, $"Code: {errorCode} Msg: {errorMsg} \n");
            }else if (Program.WarningCodes.Contains(errorCode))
            {
                Logger.Warn(Name, $"Error. Id: {id}, Code: {errorCode} Msg: {errorMsg} \n");
            }else if (errorCode == 200)
            {
                Symbol result = Program.SymbolObjects.Find(x => x.Id == id);
                if (result != null) Logger.Error(Name, $"Error. Id: {id}, Code: {errorCode} Msg: {errorMsg} {result.Ticker} \n");
            }else
            {
                Logger.Error(Name, $"Error. Id: {id}, Code: {errorCode} Msg: {errorMsg} \n");
            }
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
        public override async void commissionReport(CommissionReport commissionReport)
        {
            Logger.Verbose(Name, $"Commission report");
            commissionReport = await Converter.FixObjectValues(commissionReport);
            //if (commissionReport.RealizedPNL >= 1000000) commissionReport.RealizedPNL = 0;
            //if (commissionReport.Yield >= 1000000) commissionReport.Yield = 0;

            CommissionReportOverride commissionReportOverride = new CommissionReportOverride(commissionReport);
            Thread.Sleep(1000); // een test om te zien of execution report dan wel al in de DB zit
            CommissionRepository.InsertReport(commissionReportOverride);
            //Console.WriteLine("CommissionReport. " + commissionReport.ExecId + " - " + commissionReport.Commission + " " + commissionReport.Currency + " RPNL " + commissionReport.RealizedPNL);
            if (commissionReport.RealizedPNL != 0)
            {
                Logger.Info(Name, $"Profit: {commissionReportOverride.RealizedPNL}");
            }

        }
        //! [commissionreport]

        //! [execdetails]
        public override async void execDetails(int reqId, Contract contract, Execution execution)
        {
            Logger.Verbose(Name, $"Execution report for {contract.Symbol}");
            execution = await Converter.FixObjectValues(execution);
            ExecutionOverride executionOverride = new ExecutionOverride(execution);
            ExecutionRepository.InsertReport(contract, executionOverride);
            //Console.WriteLine("ExecDetails. " + reqId + " - " + contract.Symbol + ", " + contract.SecType + ", " + contract.Currency + " - " + execution.ExecId + ", " + execution.OrderId + ", " + execution.Shares + ", " + execution.LastLiquidity);
        }
        //! [execdetails]

        //! [orderstatus]
        public override void orderStatus(int orderId, string status, double filled, double remaining, double avgFillPrice, int permId, int parentId, double lastFillPrice, int clientId, string whyHeld, double mktCapPrice)
        {
            //Console.WriteLine("OrderStatus. Id: " + orderId + ", Status: " + status + ", Filled: " + filled + ", Remaining: " + remaining
            //    + ", AvgFillPrice: " + avgFillPrice + ", PermId: " + permId + ", ParentId: " + parentId + ", LastFillPrice: " + lastFillPrice + ", ClientId: " + clientId + ", WhyHeld: " + whyHeld + ", MktCapPrice: " + mktCapPrice);
        }
        //! [orderstatus]

        //! [openorder]
        public override async void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
             //Console.WriteLine("OpenOrder. PermID: " + order.PermId + ", ClientId: " + order.ClientId + ", OrderId: " + orderId + ", Account: " + order.Account +
             //   ", Symbol: " + contract.Symbol + ", SecType: " + contract.SecType + " , Exchange: " + contract.Exchange + ", Action: " + order.Action + ", OrderType: " + order.OrderType +
             //   ", TotalQty: " + order.TotalQuantity + ", CashQty: " + order.CashQty + ", LmtPrice: " + order.LmtPrice + ", AuxPrice: " + order.AuxPrice + ", Status: " + orderState.Status);
            OrderOverride Order = new OrderOverride(order, contract);
            Symbol SymbolObject;
            try
            {
                SymbolObject = Program.SymbolObjects.Find(i => i.Contract.Symbol == contract.Symbol);
            }
            catch (Exception)
            {
                try
                {
                    SymbolObject = Program.OwnedSymbols.Find(i => i.Contract.Symbol == contract.Symbol);
                }
                catch (Exception)
                {
                    throw;
                }
                throw;
            }
            
            if(SymbolObject != null)
            {
                if (Order.Action == "SELL") SymbolObject.SOrder = true;
                else if (Order.Action == "BUY") SymbolObject.BOrder = true;

                Order = await Converter.FixObjectValues(Order);
                //foreach (PropertyInfo prop in Order.GetType().GetProperties())
                //{                    
                //    var type = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                //    if (type == typeof(double))
                //    {
                //        try
                //        {
                //            if (Convert.ToDouble(prop.GetValue(Order, null)) >= Math.Pow(1.79,100))
                //            {
                                
                //                prop.SetValue(Order, 0.0);
                //                //Logger.Warn("Order stuff", $"{prop.Name} {prop.PropertyType} {prop.GetValue(Order, null)}");
                //            }
                //        }
                //        catch (Exception e)
                //        {
                //            Logger.Warn(Name, $"{e} in order value fix");
                //        }
                        
                //    }
                //}
                //Console.WriteLine($"upsert order {contract.Symbol}");
                OrderRepository.UpsertOrder(Order);
            }
            
            //Console.WriteLine(Order.OrderType);
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
            if(pos > 0)
            {
                Program.OwnedStocks.Add(contract.Symbol);
                ObjectId Id = new ObjectId();
                Position Position = new Position(Id, account, DateTime.Now, pos, avgCost, contract);
                PositionsRepository.UpsertPositions(Position);
            }
            
            Logger.Info(Name,$"POSITION 0 {contract.Symbol}");
        }
        //! [position]

        //! [positionend]
        public override async void positionEnd()
        {
            //Console.WriteLine("PositionEnd \n");
            foreach(string S in Program.OwnedStocks)
            {
                if (Program.SymbolList.Contains(S))
                {
                    Logger.Info(Name, $"Removed {S} from symbol list, we already have a position in {S}");
                    Program.SymbolList.Remove(S);
                }
            }
            List<Position> DbPositions = await PositionsRepository.ReadPositions(allItems: true);
            foreach(Position P in DbPositions)
            {
                try
                {
                    if (P.Contract != null && !Program.OwnedStocks.Contains(P.Contract.Symbol)) PositionsRepository.RemovePosition(P.Contract.Symbol);
                }
                catch (Exception)
                {
                    Logger.Warn(Name, $"Something went wrong when removing a symbol from DB");
                }
            }
            Program.OwnedSymbols = await SymbolManager.CreateSymbolObjects(Program.OwnedStocks, 100000);
        }
        //! [positionend]

        //! [updateportfolio]
        public override async void updatePortfolio(Contract contract, double position, double marketPrice, double marketValue, double averageCost, double unrealizedPNL, double realizedPNL, string accountName)
        {
            //Console.WriteLine("UpdatePortfolio. " + contract.Symbol + ", " + contract.SecType + " @ " + contract.Exchange
            //    + ": Position: " + position + ", MarketPrice: " + marketPrice + ", MarketValue: " + marketValue + ", AverageCost: " + averageCost
            //    + ", UnrealizedPNL: " + unrealizedPNL + ", RealizedPNL: " + realizedPNL + ", AccountName: " + accountName);

            Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Contract.Symbol == contract.Symbol);

            if (Program.SUseTrailLimitOrders && position > 0 && (unrealizedPNL / (averageCost * position)) > Program.SMinimumProfit && SymbolObject.SOrder == false)
            {
                Logger.Verbose(Name, $"{contract.Symbol} unrealized at ${unrealizedPNL} - {Math.Round(unrealizedPNL / (position * averageCost) * 100, 2)}%");

                var Results = await OrderManager.CreateOrder(symbol: contract.Symbol, action: "SELL", type: "TRAIL LIMIT", amount: position, trailStopPrice: marketPrice * (1 - (Program.STrailingPercent / 100)), priceOffset: Program.SPriceOffset, trailingPercent: Program.STrailingPercent);

                if (Results.Item1 == true) {
                    SymbolObject.SOrder = true;
                    //OrderOverride Order = await OrderManager.CreateOrder("SELL", "MKT", position);
                    contract = await ContractManager.CreateContract(contract.Symbol);

                    Program.IbClient.ClientSocket.placeOrder(Program.IbClient.NextOrderId, contract, Results.Item2);
                    Logger.Info(Name, $"Sent {Results.Item2.Action} {Results.Item2.OrderType} for {contract.Symbol}");
                    Thread.Sleep(20);
                }
            }
        }
        //! [updateportfolio]

        

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
                Program.CashBalance = float.Parse(value, System.Globalization.CultureInfo.InvariantCulture);
                Logger.Info(Name, $"Cash: ${value}");
            }else if (key.ToLower().Equals("unrealizedpnl") && currency.ToLower() == "usd" && DateTime.Now > LastPnL.AddMinutes(PnLTimeout))
            {
                LastPnL = DateTime.Now;
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

            //DateTime Time = Convert.ToDateTime(sTime);
            DateTime Time = DateTime.ParseExact(sTime, "dd-MM-yyyy HH:mm:ss", null);

            //ObjectId id = new ObjectId();

            RawDataRepository.InsertRawData(SymbolObject.Id, ((DateTimeOffset)Time).ToUnixTimeSeconds(), bar.Open, bar.High, bar.Low, bar.Close);
            //if (SymbolObject.GapCalculated == true)
            //{
            //    RawData existingData;
            //    existingData = SymbolObject.HistoricalData.Find(i => i.DateTime == Time);
            //    if(existingData != null)
            //    {
            //        Logger.Verbose(Name, $"{SymbolObject.Ticker}: datapoint already exists");
            //    }
            //    else
            //    {
            //        RawData data = new RawData(id, SymbolObject.Ticker, Time, bar.Open, bar.High, bar.Low, bar.Close);
            //        SymbolObject.HistoricalData.Add(data);
            //    }

            //}
            //else
            //{
            //    RawData data = new RawData(id, SymbolObject.Ticker, Time, bar.Open, bar.High, bar.Low, bar.Close);
            //    SymbolObject.HistoricalData.Add(data);
            //}
           

        }
        //! [historicaldata]

        //! [historicaldataend]
        public override void historicalDataEnd(int reqId, string startDate, string endDate)
        {
            //Console.WriteLine("HistoricalDataEnd - " + reqId + " from " + startDate + " to " + endDate);
            Program.GettingHistoricalData -= 1;
            //Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Id == reqId);
            //SymbolObject.ExecuteStrategy();
            //if (SymbolObject.GapCalculated == false)
            //{
            //    SymbolObject.CalculateGap();
            //} else if (SymbolObject.GapCalculated == true)
            //{
            //    SymbolObject.ExecuteStrategy(true);

            //    //vraag normale market data op voor sell
            //}
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
