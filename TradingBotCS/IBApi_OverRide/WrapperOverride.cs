using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.IBApi_OverRide
{
    class WrapperOverride : EWrapperImpl
    {
        public override void contractDetails(int reqId, ContractDetails contractDetails)
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
    }
}
