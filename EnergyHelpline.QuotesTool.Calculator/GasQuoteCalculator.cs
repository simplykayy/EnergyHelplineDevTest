using EnergyHelpline.QuotesTool.Common;
using System;
using System.Threading.Tasks;
using EnergyHelpline.QuotesTool.Common.Models;

namespace EnergyHelpline.QuotesTool.Calculator
{
    /// <summary>
    /// A type of quotes calculator specific to "Gas product" only.
    /// </summary>
    public class GasQuoteCalculator : IQuotesCalculator
    {
        public virtual async Task<double> GetAnnualCostAsync(EnergyPlan plan, QuoteParameter quoteParameter)
        {
            //Case 1: The plan under review has no expiry, so the user will not have changes to the rates during term of contract
            if (plan.InitialRateExpirationDate == null || !plan.GasFinalUnitRate.HasValue) return await GetGasUnitRateAsync(plan, quoteParameter);

            //Case 2: The plan under review has initial rate expiry, and the expiry date is still in the future
            TimeSpan? remainingDaysBeforeExpiration = plan.InitialRateExpirationDate - quoteParameter.TimeOfQuote;

            if (plan.InitialRateExpirationDate != null && remainingDaysBeforeExpiration.HasValue && remainingDaysBeforeExpiration.Value.TotalDays > 0)
            {
                double initialRateCost = await GetGasUnitRateAsync(plan, quoteParameter);
                double finalRateCost = await GetGasUnitRateAsync(plan, quoteParameter, true);

                return initialRateCost + finalRateCost;
            }

            //Case 3: The plan under review has initial rate expiry, but the expiry date has lapsed and no longer valid for initial rate benefits.
            if (plan.InitialRateExpirationDate != null && remainingDaysBeforeExpiration.HasValue && remainingDaysBeforeExpiration.Value.TotalDays <= 0)
                return await GetGasUnitRateAsync(plan, quoteParameter, true);

            return default(double);
        }

        private async Task<double> GetGasUnitRateAsync(EnergyPlan plan, QuoteParameter quoteParameter, bool isFinalRate = false)
        {
            return await Task.Run(() => 
            {
                double unitRate = default(double);
                //int daysInYear = quoteParameter.TimeOfQuote.GetDaysInYear();
                int daysInYear = 365;

                //Let's determine if this plan has an expiry date for the initial rate.
                TimeSpan? daysOnInitialUnitRate = plan.InitialRateExpirationDate - quoteParameter.TimeOfQuote;

                if (!daysOnInitialUnitRate.HasValue)
                {
                    //If we get here, we know the plan's initial rate won't change during the term of the customer's subscription.
                    //Also, this indicates there would be no initial or final rate, there would only be a base rate for the term of the contract.
                    unitRate = quoteParameter.GasUsage * plan.GasInitialUnitRate;
                }
                else
                {
                    //Ok we are here...we know with this plan, the rate will change during the term of the customer's subscription.
                    //First scenario, its a possibility that there are no numbers of days left on the initial rate (i.e the expiry date has lapsed)
                    //If this is the case, use the final rate as the base rate (this is an inverse of the use case when there is no expiry date specified)
                    if(daysOnInitialUnitRate.Value.TotalDays <= 0)
                    {
                        unitRate = GetFinalUnitRate(plan, quoteParameter, daysInYear, daysOnInitialUnitRate);
                    }
                    //Second scenario, the expiry date is still in the future, so there are still some days available for the initial rate.
                    else
                    {
                        if (!isFinalRate)
                        {
                            unitRate = ((daysOnInitialUnitRate.Value.TotalDays / daysInYear) * quoteParameter.GasUsage) * plan.GasInitialUnitRate;
                        }
                        else
                        {
                            unitRate = GetFinalUnitRate(plan, quoteParameter, daysInYear, daysOnInitialUnitRate);
                        }
                    }
                }

                return Math.Round(unitRate, 2);
            }); 
        }

        private static double GetFinalUnitRate(EnergyPlan plan, QuoteParameter quoteParameter, int daysInYear, TimeSpan? daysOnInitialUnitRate)
        {
            double daysOnFinalUnitRate = daysInYear - daysOnInitialUnitRate.Value.TotalDays;
            daysOnFinalUnitRate = daysOnFinalUnitRate / daysInYear;

            return plan.GasFinalUnitRate.CalculateFinalUnitRate(quoteParameter.GasUsage, daysOnFinalUnitRate);
        }
    }
}
