using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Net.Mail;
using System.Net;
using System.Windows;

namespace Backlog
{
    class Verification
    {
        private string server = ConfigurationManager.AppSettings["SmtpServer"];
        private int port = Convert.ToInt32(ConfigurationManager.AppSettings["Port"]);
        private string sender = ConfigurationManager.AppSettings["SenderEmailId"];
        private string password = ConfigurationManager.AppSettings["SenderPassword"];

        public void SendVerification(string user, string email, string hash)
        {
            SmtpClient smtp = new SmtpClient
            {
                Host = server,
                Port = port,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, password),
                Timeout = 20000
            };
            try
            {
                using (var message = new MailMessage(sender, email)
                {
                    Subject = "Backlog account verification",
                    Body = "Click the link below to activate your account:\nstudent.labranet.jamk.fi/~H3090/iim50300/verified.php?hash=" + hash
                })
                {
                    smtp.Send(message);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }
        
    }
}
