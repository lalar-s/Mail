using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RSSreader.Services
{
    public class SendGridSenderService
    {
        public async Task<bool> SendEmail(string receiverEmail, string title, string content)
        {
            var msg = new SendGridMessage();

            msg.SetFrom(new EmailAddress("azure_4b0b50d82f1a3594c0e39d2f72b1378f@azure.com"));

            var recipients = new List<EmailAddress>
            {
                new EmailAddress("annaburzynska2020@gmail.com"),
            };
            msg.AddTos(recipients);

            msg.SetSubject(title);
            msg.AddContent(MimeType.Html, content);

            var apiKey = "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";
            var client = new SendGridClient(apiKey);
            var response = await client.SendEmailAsync(msg);
            var a = response.Body.ReadAsStringAsync();
            return true;
        }
    }
}
