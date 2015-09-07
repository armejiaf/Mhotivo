using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Mhotivo.Data.Entities;

namespace Mhotivo.Logic
{
    public class SendEmail
    {
        public static void SendEmailToUsers(List<User> userList, string emailBodyMessage, string emailSubject)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("postmaster@app1561.mailgun.org", "MHOTIVO ORG"),
                Subject = emailSubject,
                Body = emailBodyMessage,
                IsBodyHtml = true
            };
            foreach (var user in userList)
            {
                mailMessage.To.Add(new MailAddress(user.Email));
            }
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

        public static void SendEmailToSingleUser(string to, string from, string emailBodyMessage, string emailSubject)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("from"),
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