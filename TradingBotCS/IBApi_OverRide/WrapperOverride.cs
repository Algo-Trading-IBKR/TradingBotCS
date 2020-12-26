using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.IBApi_OverRide
{
    public class WrapperOverride : EWrapperImpl
    {
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

        //! [openorder]
        public override async void openOrder(int orderId, Contract contract, Order order, OrderState orderState)
        {
            Console.WriteLine("OpenOrder. PermID: " + order.PermId + ", ClientId: " + order.ClientId + ", OrderId: " + orderId + ", Account: " + order.Account +
                ", Symbol: " + contract.Symbol + ", SecType: " + contract.SecType + " , Exchange: " + contract.Exchange + ", Action: " + order.Action + ", OrderType: " + order.OrderType +
                ", TotalQty: " + order.TotalQuantity + ", CashQty: " + order.CashQty + ", LmtPrice: " + order.LmtPrice + ", AuxPrice: " + order.AuxPrice + ", Status: " + orderState.Status);
            await OrderManager.CheckOrder(order);
            Console.WriteLine("opentest");
        }
        //! [openorder]
    }
}
