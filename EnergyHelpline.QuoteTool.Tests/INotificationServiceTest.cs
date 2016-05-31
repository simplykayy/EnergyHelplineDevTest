using EnergyHelpline.QuotesTool.Common;
using EnergyHelpline.QuotesTool.Common.Models;
using EnergyHelpline.QuotesTool.NotificationService;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnergyHelpline.QuoteTool.Tests
{
    [TestFixture]
    public class INotificationServiceTest
    {
        [Test]
        public void EmailNotificationService_ShouldRaiseNotification()
        {
            var notification = new Notification
            {
                Subject = "Test",
                Payload = "Sample Payload",
                HtmlPayload = "Sample Payload",
                Recipient = new User { EmailAddress = "Sample Email", Username = "TestUser" }
            };

            var sent = default(Notification);
            Mock<EmailNotificationService> emailNotificationSvcMock = new Mock<EmailNotificationService>();

            emailNotificationSvcMock.Setup(e => e.SendNotification(It.IsAny<Notification>()))
                .Callback((Notification msg) => { sent = msg; });

            Mock<INotificationService> notificationServiceMock = new Mock<INotificationService>();
            notificationServiceMock.Setup(n => n.SendNotification(It.IsAny<Notification>()))
                .Returns(emailNotificationSvcMock.Object.SendNotification(notification));

            emailNotificationSvcMock.Verify(en => en.SendNotification(notification), Times.Once);

            Assert.That(sent != null);
            Assert.That(sent.Payload == notification.Payload);
            Assert.That(sent.HtmlPayload == notification.HtmlPayload);

            Assert.That(sent.Subject == notification.Subject);
            Assert.That(sent.Recipient.Username == notification.Recipient.Username);
            Assert.That(sent.Recipient.EmailAddress == notification.Recipient.EmailAddress);

        }
    }
}
