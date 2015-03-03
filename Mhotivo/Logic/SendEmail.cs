﻿using System.Collections.Generic;
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
                From = new MailAddress("mhotivo@gmail.com", "MHOTIVO ORG"),
                Subject = emailSubject,
                Body = emailBodyMessage
            };


            foreach (var user in userList)
            {
                mailMessage.To.Add(new MailAddress(user.Email));
            }
            
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                UseDefaultCredentials = true,
                EnableSsl = true,
                Credentials =
                    new NetworkCredential(
                        "mhotivo@gmail.com", "abc098765"), //aca van las credenciales del coreo fuente
                DeliveryMethod = SmtpDeliveryMethod.Network
            };

            client.Send(mailMessage);

        }
    }
}