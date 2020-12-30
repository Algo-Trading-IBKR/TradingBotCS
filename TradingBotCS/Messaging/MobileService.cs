using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using TradingBotCS.HelperClasses;

namespace TradingBotCS.Messaging
{
    public static class MobileService
    {
        private static string Name = "MobileService";
        private static readonly HttpClient client = new HttpClient();

        // api key: VmGMIQOQRryF3X8Yg-iUZw==
        // api id: ff8080817662e5990176b498ed820154

        private static string Url = "https://platform.clickatell.com/v1/message";
        private static string ApiKey = "VmGMIQOQRryF3X8Yg-iUZw==";


        public static async Task SendTextMsg(string message, bool sms, bool whatsapp)
        {
            string jsonData = @"{  
                            'messages': [
                                    {
                                        'channel': 'sms',
                                        'to': '32476067619',
                                        'content': 'Test SMS Message Text'
                                    }
                                ] 
                            }";

            JObject data = JObject.Parse(jsonData);
            StringContent Content = new StringContent(data.ToString(), Encoding.UTF8, "application/json");

            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Authorization", ApiKey);
            
            //client.DefaultRequestHeaders.Add("API Key", Convert.ToBase64String(Encoding.ASCII.GetBytes(ApiKey)));

            var result = await client.PostAsync(Url, Content);
            Console.WriteLine(result);
            Logger.Info(Name, "Messages Sent");
        }

        // JAVASCRIPT EXAMPLE

        //var xhr = new XMLHttpRequest(),
        //body = JSON.stringify(
            //{
            //    "messages": [
            //        {
            //            "channel": "whatsapp",
            //            "to": "32476067619",
            //            "content": "Test WhatsApp Message Text"
            //        },
            //        {
            //            "channel": "sms",
            //            "to": "32476067619",
            //            "content": "Test SMS Message Text"
            //        }
            //    ]
            //}
        //);
        //xhr.open('POST', 'https://platform.clickatell.com/v1/message', true);
        //xhr.setRequestHeader('Content-Type', 'application/json');
        //xhr.setRequestHeader('Authorization', 'VmGMIQOQRryF3X8Yg-iUZw==');
        //xhr.onreadystatechange = function(){
        //    if (xhr.readyState == 4 && xhr.status == 200)
        //    {
        //        console.log('success');
        //    }
        //};

        //xhr.send(body);
    }
}
