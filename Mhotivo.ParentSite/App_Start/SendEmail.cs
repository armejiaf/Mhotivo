using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace Mhotivo.ParentSite
{
    public class SendEmail
    {
        public static void SendEmailToSingleUser(string to, string from, string emailBodyMessage, string emailSubject)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = emailSubject,
                Body = emailBodyMessage,
                IsBodyHtml = false
            };

            mailMessage.To.Add(to);
            var client = new SmtpClient("smtp.mailgun.org", 587)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials =
                    new NetworkCredential(
                        "postmaster@app1561.mailgun.org", "70ic5h7hd6z2"), //aca van las credenciales del coreo fuente
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(mailMessage);
        }
    }
}