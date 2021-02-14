using IBApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace TradingBotCS.Util
{
    public static class ApiConnection
    {
        private static string Name = "API Connection Util";

        public static async Task Connect()
        {
            Logger.Verbose(Name, "Connecting To API");

            Program.IbClient.ClientSocket.eConnect(Program.Ip, Program.Port, Program.ApiId);
            Program.IbReader = new EReader(Program.IbClient.ClientSocket, Program.IbClient.Signal);
            Program.IbReader.Start();

            new Thread(() =>
            {
                while (Program.IbClient.ClientSocket.IsConnected())
                {
                    Program.IbClient.Signal.waitForSignal();
                    Program.IbReader.processMsgs();
                }
            })
            { IsBackground = true }.Start();
        }

        public static async Task AccountUpdates()
        {
            Logger.Verbose(Name, "Requesting Account Updates");
            new Thread(() =>
            {
                if (Program.IbClient.ClientSocket.IsConnected())
                {
                    Program.IbClient.ClientSocket.reqAccountUpdates(true, Program.AccountId);
                    Program.IbClient.ClientSocket.reqPositions();
                }
            })
            { IsBackground = false }.Start();
        }
    }
}
