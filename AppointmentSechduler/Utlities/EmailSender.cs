using Mailjet.Client;
using System;
using Mailjet.Client.Resources;
using Microsoft.AspNetCore.Identity.UI.Services;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace AppointmentSechduler.Utlities
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            MailjetClient client = new MailjetClient("535a087188549170e0d70df040d25fb2", "ab4e0639f9c3a3ef01af3650e6153a6d")
            {
            };

            MailjetRequest request = new MailjetRequest
            {
                Resource = Send.Resource,
            }
                .Property(Send.FromEmail, "jmm.rafiullahafridi@gmail.com")
                .Property(Send.FromName, "Appointment Scheduler")
                .Property(Send.Subject, subject)
                .Property(Send.HtmlPart, htmlMessage)
                .Property(Send.Recipients, new JArray {
                new JObject {
                 {"Email", email}
                 }
       });
            MailjetResponse response = await client.PostAsync(request);
           
        }
    }
}
