using BCrypt.Net;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Util
{
    public static class Encryption
    {
        // 10 Rounds for processing time below 100ms
        // 13 Rounds for processing time below 1000ms
        public static async Task<string> Hash(string input)
        {
            string Hash = BCrypt.Net.BCrypt.EnhancedHashPassword(input, hashType: HashType.SHA512);
            Console.WriteLine(Hash);
            return Hash;
        }

        public static async Task<bool> Verify(string hash, string input)
        {
            bool Result = BCrypt.Net.BCrypt.EnhancedVerify(input, hash, hashType: HashType.SHA512);
            return Result;
        }

        public static async Task Test(string input)
        {
            string Hash = BCrypt.Net.BCrypt.EnhancedHashPassword(input, hashType: HashType.SHA512);
            bool Test = BCrypt.Net.BCrypt.EnhancedVerify(input, Hash, hashType: HashType.SHA512);
            Console.WriteLine(Hash);
            Console.WriteLine(Test);
        }


    }
}
