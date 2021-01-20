using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CsvHelper;
using TradingBotCS.DataModels;

namespace DataCollectorCS
{
    public static class Csv
    {
        

        public static async Task WriteToCsv(string symbol, List<RawData> data)
        {
            MemoryStream mem = new MemoryStream();
            StreamWriter writer = new StreamWriter($"{symbol}.csv");
            CsvWriter csvWriter = new CsvWriter(writer, CultureInfo.CurrentCulture);

            csvWriter.WriteField("Time");
            csvWriter.WriteField("Low");
            csvWriter.WriteField("Open");
            csvWriter.WriteField("High");
            csvWriter.WriteField("Close");
            csvWriter.NextRecord();
            foreach (RawData R in data)
            {
                csvWriter.WriteField(R.DateTime);
                csvWriter.WriteField(R.Low);
                csvWriter.WriteField(R.Open);
                csvWriter.WriteField(R.High);
                csvWriter.WriteField(R.Close);
                csvWriter.NextRecord();
            }

            writer.Flush();
            var result = Encoding.UTF8.GetString(mem.ToArray());

            
        }




    }
}
