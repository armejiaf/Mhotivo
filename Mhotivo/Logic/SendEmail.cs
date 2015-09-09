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
            var client = new SmtpClient("smtp.mailgun.com", 587)
            {
                UseDefaultCredentials = false,
                EnableSsl = true,
                Credentials =
                    new NetworkCredential("mhotivo@sandbox172d6462cbee435f9a8cd0a91d1f6619.mailgun.org", "password"), //aca van las credenciales del coreo fuente
                DeliveryMethod = SmtpDeliveryMethod.Network
            };
            client.Send(mailMessage);
        }

    }
}