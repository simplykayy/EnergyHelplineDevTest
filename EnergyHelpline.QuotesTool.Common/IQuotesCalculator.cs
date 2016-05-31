using EnergyHelpline.QuotesTool.Common.Models;
using System.Threading.Tasks;

namespace EnergyHelpline.QuotesTool.Common
{
    /// <summary>
    /// Type that provides a public behaviour for all supported products.
    /// For this use case, these are gas and electricity. To support more products,
    /// the new product only has to implement this interface and it will be implicitly supported by the tool.
    /// </summary>
    public interface IQuotesCalculator
    {
        Task<double> GetAnnualCostAsync(EnergyPlan plan, QuoteParameter quoteParameter);
    }
}
