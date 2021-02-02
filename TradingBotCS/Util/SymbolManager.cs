using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Util
{
    public class SymbolManager
    {
        private static string Name = "Symbol Manager";

        public static List<Symbol> CreateSymbolObjects(List<string> symbolList)
        {
            Logger.Verbose(Name, "Creating Symbol Objects");

            List<Symbol> Result = new List<Symbol>();
            for (int i = 0; i < symbolList.Count; i++)
            {
                Symbol s = new Symbol(symbolList[i], i);
                Result.Add(s);
            }
            return Result;
        }
    }
}
