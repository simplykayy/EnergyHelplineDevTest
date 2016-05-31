using EnergyHelpline.QuotesTool.Common;
using Moq;
using NUnit.Framework;
using System;

namespace EnergyHelpline.QuoteTool.Tests
{
    [TestFixture]
    public class IInputServiceTest
    {
        [Test]
        public void ConsoleInputService_ShouldWriteMessage()
        {
            var quoteParameter = new QuotesTool.Common.Models.QuoteParameter
            {
                ElectricityUsage = 100,
                GasUsage = 50,
                TimeOfQuote = new DateTime(2016, 5, 29),
                QuoteUser = new QuotesTool.Common.Models.User { EmailAddress = "tests@tdd.developer", Username = "tdd" }
            };

            Mock<IInputService> inputServiceMock = new Mock<IInputService>();
            inputServiceMock.Setup(iC => iC.ReadCommand()).Returns("Test Command");
            inputServiceMock.Setup(ic => ic.ReadParameters()).Returns(quoteParameter);

            string testCmd = inputServiceMock.Object.ReadCommand();
            QuotesTool.Common.Models.QuoteParameter mockParams = inputServiceMock.Object.ReadParameters();

            inputServiceMock.Verify(i => i.ReadCommand(), Times.Once);
            inputServiceMock.Verify(i => i.ReadParameters(), Times.Once);

            Assert.AreEqual(testCmd, "Test Command");
            Assert.IsTrue(mockParams != null);
            Assert.IsTrue(mockParams.ElectricityUsage == quoteParameter.ElectricityUsage);

            Assert.IsTrue(mockParams.GasUsage == quoteParameter.GasUsage);
            Assert.IsTrue(mockParams.TimeOfQuote == quoteParameter.TimeOfQuote);
            Assert.That(mockParams.QuoteUser != null && mockParams.QuoteUser.Username == quoteParameter.QuoteUser.Username);
            Assert.That(mockParams.QuoteUser.EmailAddress == quoteParameter.QuoteUser.EmailAddress);
        }
    }
}
