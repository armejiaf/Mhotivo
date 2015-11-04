using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using Mhotivo.Data.Entities;

namespace Mhotivo.Implement.Services
{
    public class EmailService
    {
        public static void SendEmailToUsers(List<User> userList, Notification notification)
        {
            foreach (var user in userList)
            {
                SendEmailToUser(user, notification);
            }
        }

        public static void SendEmailToUser(User user, Notification notification)
        {
            var mailMessage = new MailMessage
            {
                From = new MailAddress("postmaster@app1561.mailgun.org", "FUNDACION MHOTIVO"),
                Subject = notification.Title,
                Body ="Se ha creado una notificacion en la cual usted ha sido incluido. Mensaje: " +
                        notification.Message,
                IsBodyHtml = true
            };
            mailMessage.To.Add(new MailAddress(user.Email));
            var client = new SmtpClient("smtp.mailgun.org", 587)
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