using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.DataModels;
using TradingBotCS.Util;

namespace TradingBotCS.Messaging
{
    public static class MobileService
    {
        private static string Name = "MobileService";

        // api key: VmGMIQOQRryF3X8Yg-iUZw==
        // api id: ff8080817662e5990176b498ed820154

        private static string Url = "https://platform.clickatell.com/v1/message";
        private static string ApiKey = "VmGMIQOQRryF3X8Yg-iUZw==";

        public static async Task SendTextMsg(List<string> contents, List<string> numbers)
        {
            
            for (int i = 0; i < contents.Count; i++)
            {
                List<TextMessage> Messages = new List<TextMessage>();
                TextMessage Message = new TextMessage(contents[i], "sms", numbers[i]);
                Messages.Add(Message);
            
                string RawData = JsonConvert.SerializeObject(new{ 
                    messages = Messages
                });
                Console.WriteLine(RawData);

                var Data = Encoding.UTF8.GetBytes(RawData.ToString());

                WebRequest request = WebRequest.Create(Url);

                request.Method = "POST";
                request.ContentType = "application/json";
                request.Headers.Add("Authorization", ApiKey);
                request.ContentLength = Data.Length;

                using (Stream stream = request.GetRequestStream())
                {
                    stream.Write(Data, 0, Data.Length);
                }
                try
                {
                    WebResponse response = await request.GetResponseAsync();
                    using (StreamReader reader = new StreamReader(response.GetResponseStream()))
                    {
                        string responseContent = reader.ReadToEnd();
                        JObject adResponse =
                            JsonConvert.DeserializeObject<JObject>(responseContent);
                        Logger.Verbose(Name, adResponse.ToString());
                    }
                }
                catch (WebException webException)
                {
                    if (webException.Response != null)
                    {
                        using (StreamReader reader = new StreamReader(webException.Response.GetResponseStream()))
                        {
                            string responseContent = reader.ReadToEnd();
                            Logger.Error(Name, JsonConvert.DeserializeObject<JObject>(responseContent).ToString()); 
                        }
                    }
                }
            }
        }

        public static async Task SendTextMsg(string content, string number)
        {
            List<string> Messages = new List<string>() { content };
            List<string> Numbers = new List<string>() { number };
            SendTextMsg(Messages, Numbers);
        }

        public static async Task SendTextMsg(string content, List<string> numbers)
        {
            List<string> Messages = new List<string>();
            foreach(string s in numbers)
            {
                Messages.Add(content);
            }
            SendTextMsg(Messages, numbers);
        }

    }
}
