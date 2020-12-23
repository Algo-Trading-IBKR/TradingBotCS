using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace TradingBotCS.Email
{
    public static class MailService
    {
        public static void SendMail()
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("jytradingbot@hotmail.com");
                message.To.Add(new MailAddress("jorenvangoethem@hotmail.com"));
                message.Subject = "Test";
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = "test123456";
                smtp.Port = 587;
                smtp.Host = "smtp.live.com"; //for outlook host  
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("jytradingbot@hotmail.com", "PythonTradingBotJY");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception) { }
        }
    }
}
