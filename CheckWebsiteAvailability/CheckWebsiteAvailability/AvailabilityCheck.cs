using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Net.Mail;

namespace CheckWebsiteAvailability
{
    class AvailabilityCheck
    {
        public string CurrentStatus()
        {
            string[] lines;
            List<string> FailServers = new List<string>();
            const string RootFolder = @"D:\Programming\CheckWebsiteAvailabilityC#\";
            const string textFile = RootFolder+ "URLs.txt";
            if (File.Exists(textFile))
            {
                lines = File.ReadAllLines(textFile);
                foreach (string line in lines)
                {
                    HttpWebRequest request = (HttpWebRequest)WebRequest.Create(line);
                    request.Timeout = 15000;
                    try
                    {
                        using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                        {
                            var res =response.StatusCode == HttpStatusCode.OK;
                            Console.WriteLine(res.ToString() + "->" + line);
                        }
                    }
                    catch (WebException)
                    {
                        FailServers.Add(line);
                        Console.WriteLine("False" + "->" + line);
                    }
                }
                SendMail(FailServers);
            }
            return "Fail";
            //request.Timeout = 15000;
            //request.Method = "HEAD"; // As per Lasse's comment
        }
        private void SendMail(List<string> WebUrl)
        {
            var senderEmail = new MailAddress("noreply@egyptiancurebank.com", "منظومة بنك الشفاء المصرى");
            var receiverEmail = new MailAddress("m.abdullah@egyptiancurebank.com", "Receiver");
            var password = "123456789";
            var sub = "WebSite Is Down";
            StringBuilder body = new StringBuilder();
            body.Append("<html> <head> <title>One Of WebSites Is Down Now</title> </head> <body style='background-color: #ccfff9;'>");
            foreach (var url in WebUrl)
            {
                Console.WriteLine(url.ToString());
                body.Append("<p>"+"Server: "+ url.ToString()+"</p>");
                body.Append("<h3 style='color:blue;text-align:center;'> " + url + "</h3>");
            }
            body.Append(" <p style='text-align:right;'>The Above Web Sites Is Down</p> </body> </html>");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password),

            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {
                Subject = sub,
                Body = body.ToString(),
                IsBodyHtml = true,
            })
            {
                smtp.Send(mess);
            }
        }
        public string GetCurrentStatus(string Url)
        {
            string StatusCode = "";
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            //request.Timeout = 15000;
            //request.Method = "HEAD"; // As per Lasse's comment
            try
            {
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    return response.StatusCode.ToString();
                }
            }
            catch(System.IO.IOException)
            {
                return "Connection Fail To WebSite";
            }
            catch (WebException EX)
            {
                return EX.ToString() ;
            }
        }
    }
}
