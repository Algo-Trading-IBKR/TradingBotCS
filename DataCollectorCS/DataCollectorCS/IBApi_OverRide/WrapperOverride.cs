using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.IBApi_OverRide
{
    public class WrapperOverride : EWrapperImpl
    {
        private static string Name = "WrapperOverride";

        //! [error]
        public override void error(int id, int errorCode, string errorMsg)
        {
            //Console.WriteLine("Error. Id: " + id + ", Code: " + errorCode + ", Msg: " + errorMsg + "\n");
            
            if (errorCode == 162 || errorCode == 321 || errorCode == 322 || errorCode == 200)
            {
                Logger.Warn(Name, $"Error. Id: {id}, Code: {errorCode} Msg: {errorMsg} \n");
                if (errorCode == 200)
                {
                    Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Ids.Contains(id) == true);
                    SymbolObject.Enabled = false;
                }
                Thread.Sleep(20);
                Program.GettingData -= 1;
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

        //! [contractdetailsend]
        public override void contractDetailsEnd(int reqId)
        {
            //Console.WriteLine("ContractDetailsEnd. " + reqId + "\n");
        }
        //! [contractdetailsend]

        //! [historicaldata]
        public override void historicalData(int reqId, Bar bar)
        {
            //Console.WriteLine("HistoricalData. " + reqId + " - Time: " + bar.Time + ", Open: " + bar.Open + ", High: " + bar.High + ", Low: " + bar.Low + ", Close: " + bar.Close + ", Volume: " + bar.Volume + ", Count: " + bar.Count + ", WAP: " + bar.WAP);

            //Logger.Info("barTime: " , bar.Time);
            //Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Id == reqId);
            Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Ids.Contains(reqId) == true);
            DateTime Time;
            if (bar.Time.Contains(':'))
            {
                string sTime = bar.Time.Insert(4, "-");
                sTime = sTime.Insert(7, "-");

                string[] words = sTime.Split('-');
                string[] morewords = words[2].Split(' ');

                sTime = morewords[0] + "-" + words[1] + "-" + words[0] + " " + morewords[2];
                //DateTime Time = Convert.ToDateTime(sTime);
                Time = DateTime.ParseExact(sTime, "dd-MM-yyyy HH:mm:ss", null);
            }
            else 
            {
                string sTime = bar.Time.Insert(4, "-");
                sTime = sTime.Insert(7, "-");

                string[] words = sTime.Split('-');

                sTime = words[2] + "-" + words[1] + "-" + words[0] + " " + "00:00:00";

                //DateTime Time = Convert.ToDateTime(sTime);
                Time = DateTime.ParseExact(sTime, "dd-MM-yyyy HH:mm:ss", null);
            }

            RawData existingData;
            existingData = SymbolObject.RawDataList.Find(i => i.DateTime == Time);

            if (existingData != null)
            {
                Logger.Warn(Name, $"{SymbolObject.Ticker}: datapoint {Time} already exists");
            }
            else
            {
                RawData data = new RawData(Time, bar.Open, bar.High, bar.Low, bar.Close);
                SymbolObject.RawDataList.Add(data);
            }
        }
        //! [historicaldata]

        //! [historicaldataend]
        public override void historicalDataEnd(int reqId, string startDate, string endDate)
        {
            Console.WriteLine("HistoricalDataEnd - " + reqId + " from " + startDate + " to " + endDate);
            Program.GettingData -= 1;
            Symbol SymbolObject = Program.SymbolObjects.Find(i => i.Id == reqId);
        }
        //! [historicaldataend]

        //! [contractdetails]
        public override async void contractDetails(int reqId, ContractDetails contractDetails)
        {
            //Console.WriteLine("ContractDetails begin. ReqId: " + reqId);
            //printContractMsg(contractDetails.Contract);
            //printContractDetailsMsg(contractDetails);
            //Console.WriteLine("ContractDetails end. ReqId: " + reqId);
        }
        //! [contractdetails]

    }
}
