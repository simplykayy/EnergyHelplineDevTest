using EnergyHelpline.QuotesTool.Calculator;
using EnergyHelpline.QuotesTool.Common;
using EnergyHelpline.QuotesTool.Common.Models;
using Moq;
using NUnit.Framework;
using System.Threading.Tasks;

namespace EnergyHelpline.QuoteTool.Tests
{
    [TestFixture]
    public class IQuotesCalculatorTest
    {
        EnergyPlan testPlan;
        QuoteParameter testParameter;
        Mock<IQuotesCalculator> quotesCalculatorMock;

        [SetUp]
        public void InitTest()
        {
            quotesCalculatorMock = new Mock<IQuotesCalculator>();
        }

        #region Gas Quote Calculator Test
        [Test]
        public async Task GasQuoteCalculator_ShouldComputeUsingOnlyInitialRates_NoInitialRateExpiry()
        {
            testPlan = new EnergyPlan
            {
                TariffName = "Standard",
                GasInitialUnitRate = 0.65
            };

            testParameter = new QuoteParameter
            {
                GasUsage = 1500,
                TimeOfQuote = new System.DateTime(2016, 6, 1),
                QuoteUser = new User { EmailAddress = string.Empty, Username = "UnitTest" }
            };

            quotesCalculatorMock.Setup(qc => qc.GetAnnualCostAsync(testPlan, testParameter))
                .Returns(new GasQuoteCalculator().GetAnnualCostAsync(testPlan, testParameter));

            double annualCost = await quotesCalculatorMock.Object.GetAnnualCostAsync(testPlan, testParameter);
            quotesCalculatorMock.Verify(qm => qm.GetAnnualCostAsync(testPlan, testParameter), Times.Once);
            Assert.AreEqual(annualCost, 975);
        }

        [Test]
        public async Task GasQuoteCalculator_ShouldComputeUsingOnlyFinalRates_ExpiredInitialRate()
        {
            testPlan = new EnergyPlan
            {
                TariffName = "Energy Saver",
                GasFinalUnitRate = 0.50,
                InitialRateExpirationDate = new System.DateTime(2016, 6, 1)
            };

            testParameter = new QuoteParameter
            {
                GasUsage = 1500,
                TimeOfQuote = new System.DateTime(2016, 6, 1),
                QuoteUser = new User { EmailAddress = string.Empty, Username = "UnitTest" }
            };

            quotesCalculatorMock.Setup(qc => qc.GetAnnualCostAsync(testPlan, testParameter))
                .Returns(new GasQuoteCalculator().GetAnnualCostAsync(testPlan, testParameter));

            double annualCost = await quotesCalculatorMock.Object.GetAnnualCostAsync(testPlan, testParameter);
            quotesCalculatorMock.Verify(qm => qm.GetAnnualCostAsync(testPlan, testParameter), Times.Once);
            Assert.AreEqual(annualCost, 750);
        }

        [Test]
        public async Task GasQuoteCalculator_ShouldComputeInitialAndFinalRates()
        {
            testPlan = new EnergyPlan
            {
                TariffName = "Energy Saver",
                GasInitialUnitRate = 0.25,
                GasFinalUnitRate = 0.50,
                InitialRateExpirationDate = new System.DateTime(2017, 1, 1)
            };

            testParameter = new QuoteParameter
            {
                GasUsage = 1500,
                TimeOfQuote = new System.DateTime(2016, 6, 1),
                QuoteUser = new User { EmailAddress = string.Empty, Username = "UnitTest" }
            };

            quotesCalculatorMock.Setup(qc => qc.GetAnnualCostAsync(testPlan, testParameter))
                .Returns(new GasQuoteCalculator().GetAnnualCostAsync(testPlan, testParameter));

            double annualCost = await quotesCalculatorMock.Object.GetAnnualCostAsync(testPlan, testParameter);
            quotesCalculatorMock.Verify(qm => qm.GetAnnualCostAsync(testPlan, testParameter), Times.Once);
            Assert.AreEqual(annualCost, 530.13);
        }
        #endregion

        #region Electricity Quote Calculator
        [Test]
        public async Task ElectricityQuoteCalculator_ShouldComputeUsingOnlyInitialRates_NoInitialRateExpiry()
        {
            testPlan = new EnergyPlan
            {
                TariffName = "Standard",
                ElectricityInitialUnitRate = 0.80
            };

            testParameter = new QuoteParameter
            {
                ElectricityUsage = 1500,
                TimeOfQuote = new System.DateTime(2016, 6, 1),
                QuoteUser = new User { EmailAddress = string.Empty, Username = "UnitTest" }
            };

            quotesCalculatorMock.Setup(qc => qc.GetAnnualCostAsync(testPlan, testParameter))
                .Returns(new ElectricityQuoteCalculator().GetAnnualCostAsync(testPlan, testParameter));

            double annualCost = await quotesCalculatorMock.Object.GetAnnualCostAsync(testPlan, testParameter);
            quotesCalculatorMock.Verify(qm => qm.GetAnnualCostAsync(testPlan, testParameter), Times.Once);
            Assert.AreEqual(annualCost, 1200);
        }

        [Test]
        public async Task ElectricityQuoteCalculator_ShouldComputeUsingOnlyFinalRates_ExpiredInitialRate()
        {
            testPlan = new EnergyPlan
            {
                TariffName = "Energy Saver",
                ElectricityFinalUnitRate = 0.60,
                InitialRateExpirationDate = new System.DateTime(2016, 6, 1)
            };

            testParameter = new QuoteParameter
            {
                ElectricityUsage = 1500,
                TimeOfQuote = new System.DateTime(2016, 6, 1),
                QuoteUser = new User { EmailAddress = string.Empty, Username = "UnitTest" }
            };

            quotesCalculatorMock.Setup(qc => qc.GetAnnualCostAsync(testPlan, testParameter))
                .Returns(new ElectricityQuoteCalculator().GetAnnualCostAsync(testPlan, testParameter));

            double annualCost = await quotesCalculatorMock.Object.GetAnnualCostAsync(testPlan, testParameter);
            quotesCalculatorMock.Verify(qm => qm.GetAnnualCostAsync(testPlan, testParameter), Times.Once);
            Assert.AreEqual(annualCost, 900);
        }

        [Test]
        public async Task ElectricityQuoteCalculator_ShouldComputeInitialAndFinalRates()
        {
            testPlan = new EnergyPlan
            {
                TariffName = "Energy Saver",
                ElectricityInitialUnitRate = 0.30,
                ElectricityFinalUnitRate = 0.60,
                InitialRateExpirationDate = new System.DateTime(2017, 1, 1)
            };

            testParameter = new QuoteParameter
            {
                ElectricityUsage = 3000,
                TimeOfQuote = new System.DateTime(2016, 6, 1),
                QuoteUser = new User { EmailAddress = string.Empty, Username = "UnitTest" }
            };

            quotesCalculatorMock.Setup(qc => qc.GetAnnualCostAsync(testPlan, testParameter))
                .Returns(new ElectricityQuoteCalculator().GetAnnualCostAsync(testPlan, testParameter));

            double annualCost = await quotesCalculatorMock.Object.GetAnnualCostAsync(testPlan, testParameter);
            quotesCalculatorMock.Verify(qm => qm.GetAnnualCostAsync(testPlan, testParameter), Times.Once);
            Assert.AreEqual(annualCost, 1272.33);
        }
        #endregion

        #region Gas and Electricity Quote Combined Tests
        [Test]
        public async Task GasElectricityQuoteCalculator_ShouldComputeInitialAndFinalRates()
        {
            testPlan = new EnergyPlan
            {
                TariffName = "Energy Saver",
                GasInitialUnitRate = 0.25,
                GasFinalUnitRate = 0.50,
                ElectricityInitialUnitRate = 0.30,
                ElectricityFinalUnitRate = 0.60,
                InitialRateExpirationDate = new System.DateTime(2017, 1, 1)
            };

            testParameter = new QuoteParameter
            {
                GasUsage = 1500,
                ElectricityUsage = 3000,
                TimeOfQuote = new System.DateTime(2016, 6, 1),
                QuoteUser = new User { EmailAddress = string.Empty, Username = "UnitTest" }
            };

            quotesCalculatorMock.Setup(qc => qc.GetAnnualCostAsync(testPlan, testParameter))
                .Returns(new ElectricityQuoteCalculator().GetAnnualCostAsync(testPlan, testParameter));

            double annualCostElectricity = await quotesCalculatorMock.Object.GetAnnualCostAsync(testPlan, testParameter);
            quotesCalculatorMock.Verify(qm => qm.GetAnnualCostAsync(testPlan, testParameter), Times.Once);

            quotesCalculatorMock.Setup(qc => qc.GetAnnualCostAsync(testPlan, testParameter))
                .Returns(new GasQuoteCalculator().GetAnnualCostAsync(testPlan, testParameter));

            double annualCostGas = await quotesCalculatorMock.Object.GetAnnualCostAsync(testPlan, testParameter);
            quotesCalculatorMock.Verify(qm => qm.GetAnnualCostAsync(testPlan, testParameter), Times.Exactly(2));

            Assert.AreEqual(annualCostGas, 530.13);
            Assert.AreEqual(annualCostElectricity, 1272.33);
            Assert.AreEqual(annualCostGas + annualCostElectricity, 530.13 + 1272.33);
        }
        #endregion
    }
}
