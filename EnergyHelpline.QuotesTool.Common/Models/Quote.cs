using System;

namespace EnergyHelpline.QuotesTool.Common.Models
{
    /// <summary>
    /// Entity for the computed quote. It implements IComparable, 
    /// to enable self-description for certain operations such as sorting and comparisons.
    /// </summary>
    public class Quote : IComparable<Quote>
    {
        public DateTime Date { get; set; }
        public double GasUsage { get; set; }
        public double ElectricityUsage { get; set; }
        public string CheapestTariff { get; set; }
        public double AnnualCost { get; set; }

        public int CompareTo(Quote other)
        {
            if (other == null)
                return 1;

            return AnnualCost.CompareTo(other.AnnualCost);
        }
    }
}
