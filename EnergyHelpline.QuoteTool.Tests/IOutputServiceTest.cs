using EnergyHelpline.QuotesTool.Common;
using Moq;
using NUnit.Framework;

namespace EnergyHelpline.QuoteTool.Tests
{
    [TestFixture]
    public class IOutputServiceTest
    {
        [Test]
        public void ShouldOutputMessage()
        {
            string message = "Sample Output Message";
            string output = string.Empty;

            Mock<IOutputService> outputServiceMock = new Mock<IOutputService>();

            outputServiceMock.Setup(o => o.WriteMessage(It.IsAny<string>()))
                .Callback((string msg) =>  { output = msg; })
                .Verifiable();

            outputServiceMock.Object.WriteMessage(message);
            Assert.AreEqual(message, output);

        }
    }
}
