using EnergyHelpline.QuotesTool.Common;
using EnergyHelpline.QuotesTool.Common.Models;
using SendGrid;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace EnergyHelpline.QuotesTool.NotificationService
{
    public class EmailNotificationService : INotificationService
    {
        public virtual async Task SendNotification(Notification notification)
        {
            string senderEmail = ConfigurationManager.AppSettings[NotificationSettings.Default.SenderEmail];
            string senderDisplayName = ConfigurationManager.AppSettings[NotificationSettings.Default.SenderDisplayName];

            if(string.IsNullOrWhiteSpace(senderEmail) || string.IsNullOrWhiteSpace(senderDisplayName))
            {
                throw new InvalidOperationException(NotificationSettings.Default.MissingSenderKeyError);
            }

            MailAddress sender = new MailAddress(senderEmail, senderDisplayName);
            MailAddress[] recipients = new MailAddress[] { new MailAddress(notification.Recipient.EmailAddress, notification.Recipient.Username) };
            SendGridMessage message = new SendGridMessage(sender, recipients, notification.Subject, string.Empty, notification.Payload);
            message.Html = notification.HtmlPayload;

            if (!string.IsNullOrWhiteSpace(notification.AttachmentFilePath))
            {
                message.AddAttachment(notification.AttachmentFilePath);
            }

            string apiKey = ConfigurationManager.AppSettings[NotificationSettings.Default.ApiKey];
            Web httpTransport = new Web(apiKey);
            await httpTransport.DeliverAsync(message);
        }
    }
}
