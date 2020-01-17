using NaucnaCentrala.Interfaces.Services;
using System;
using System.Net;
using System.Net.Mail;

namespace NaucnaCentrala.Core.Services
{
    public class EmailService : IEmailService
    {
        public bool SendEmail(string subject, string message)
        {
            try
            {
                var credentials = new NetworkCredential("kovacevic.j941@gmail.com", "TotheMoonandBack94");
                var mail = new MailMessage()
                {
                    From = new MailAddress("kovacevic.j941@gmail.com"),
                    Subject = subject,
                    Body = message
            };
                mail.IsBodyHtml = true;
                mail.To.Add(new MailAddress("kovacevic.j941@gmail.com"));
                var client = new SmtpClient()
                {
                    Port = 587,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false,
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    Credentials = credentials
                };

                client.Send(mail);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
