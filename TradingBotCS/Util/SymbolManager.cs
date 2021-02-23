using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.Database;
using TradingBotCS.DataModels;

namespace TradingBotCS.Util
{
    public class SymbolManager
    {
        private static string Name = "Symbol Manager";

        public static List<Symbol> CreateSymbolObjects(List<string> symbolList, int startingId = 0) // startingid is used for different lists of symbols to avoid conflicts
        {
            Logger.Verbose(Name, "Creating Symbol Objects");

            List<Symbol> Result = new List<Symbol>();

            for (int i = startingId; i < (symbolList.Count + startingId); i++)
            {
                Symbol s = new Symbol(symbolList[i-startingId], i);
                Result.Add(s);
            }
            return Result;
        }
    }
}
