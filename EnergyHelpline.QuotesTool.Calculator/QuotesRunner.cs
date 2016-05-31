using System.Threading.Tasks;
using EnergyHelpline.QuotesTool.Common;
using EnergyHelpline.QuotesTool.Common.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using System.IO;
using System.Reflection;

namespace EnergyHelpline.QuotesTool.Calculator
{
    /// <summary>
    /// Entry point to the quotes application
    /// </summary>
    public class QuotesRunner : IQuotesRunner
    {
        #region Dependent Services
        private IQuotesCalculator[] _quoteCalculators;
        private IInputService _inputService;
        private IOutputService _outputService;
        private INotificationService _notificationService;
        #endregion

        public QuotesRunner(IQuotesCalculator[] quoteCalculators, IInputService inputService, 
            IOutputService outputService, INotificationService notificationService)
        {
            _quoteCalculators = quoteCalculators;
            _inputService = inputService;
            _outputService = outputService;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Starts application post-ioc type registrations
        /// </summary>
        public virtual async void Run()
        {
            _outputService.WriteMessage("===================================================================================");
            _outputService.WriteMessage("=                                                                                 =");
            _outputService.WriteMessage("=                 Energy HelpLine DevTest Quote Comparison Tool                   =");
            _outputService.WriteMessage("=                                                                                 =");
            _outputService.WriteMessage("===================================================================================");
            _outputService.WriteMessage(string.Empty);

            //Ideally the energy plan list would be obtained by initiating an async service or database call
            //but for this use case, using an in-memory collection would be sufficient.
            IReadOnlyCollection<EnergyPlan> energyPlans = EnergyPlan.InitialisePlans();

            List<Quote> results = new List<Quote>();
            QuoteParameter parameters = _inputService.ReadParameters();

            //In situations where there is a huge list of energy plans, the foreach can be swapped 
            //with Parallel.ForEach to improve performance, but for this use case it's an overkill.
            foreach (EnergyPlan plan in energyPlans)
            {
                Quote quoteResult = await CalculateAnnualCostPerPlanAsync(plan, parameters);
                results.Add(quoteResult);
            }

            results.Sort();
            Quote cheapestQuote = results.First();

            string consolePayload = string.Format("Date: {0}" + Environment.NewLine + "Gas Usage: {1}" + Environment.NewLine +
                "Electricity Usage: {2}" + Environment.NewLine + "Cheapest Tariff: {3}" + Environment.NewLine + "Annual Cost: £{4}",
                parameters.TimeOfQuote, parameters.GasUsage, parameters.ElectricityUsage, cheapestQuote.CheapestTariff, cheapestQuote.AnnualCost);

            _outputService.WriteMessage(consolePayload);

            string htmlPayload = await ParseHtmlPayload(parameters, cheapestQuote);
            await _notificationService.SendNotification(new Notification
            {
                Payload = consolePayload,
                HtmlPayload = htmlPayload,
                Recipient = parameters.QuoteUser,
                Subject = "New Quote - Energy Helpline Test Tool"
            });
        }

        private async Task<string> ParseHtmlPayload(QuoteParameter parameters, Quote cheapestQuote)
        {
            string htmlPayload = string.Empty;

            using (StreamReader streamReader = new StreamReader(GetNotificationTemplatePath(QuoteRunnerSettings.Default.TemplateName)))
            {
                htmlPayload = await streamReader.ReadToEndAsync();
                htmlPayload = htmlPayload.Replace("{QuoteUser}", parameters.QuoteUser.Username);

                htmlPayload = htmlPayload.Replace("{CheapestQuote}", 
                    string.Format(QuoteRunnerSettings.Default.HtmlPayload,
                        parameters.TimeOfQuote, 
                        parameters.GasUsage, 
                        parameters.ElectricityUsage, 
                        cheapestQuote.CheapestTariff, 
                        cheapestQuote.AnnualCost));

                htmlPayload = htmlPayload.Replace("{QuoteSignature}", QuoteRunnerSettings.Default.QuoteSignature);
            }

            return htmlPayload;
        }

        /// <summary>
        /// Utility method to help calculate annual cost per plan for all specified products
        /// i.e in the given use case => gas and electricity. But it's flexible enough to handle any product
        /// </summary>
        /// <param name="plan">Energy plan that specifies the rates that will be used in computing cost</param>
        /// <param name="quoteParameter">User provided data such as usage(gas or electricity), user name and email</param>
        /// <returns>Computed quote based on energy plan and user's usage</returns>
        private async Task<Quote> CalculateAnnualCostPerPlanAsync(EnergyPlan plan, QuoteParameter quoteParameter)
        {
            double finalCost = default(double);
            foreach (IQuotesCalculator calculator in _quoteCalculators)
            {
                finalCost += await calculator.GetAnnualCostAsync(plan, quoteParameter);
            }

            return new Quote
            {
                AnnualCost = finalCost,
                CheapestTariff = plan.TariffName,
                Date = quoteParameter.TimeOfQuote,
                ElectricityUsage = quoteParameter.ElectricityUsage,
                GasUsage = quoteParameter.GasUsage
            };
        }

        private string GetNotificationTemplatePath(string templateName)
        {
            string binDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
            return Path.Combine(new Uri(binDir).LocalPath, templateName);
        }
    }
}
