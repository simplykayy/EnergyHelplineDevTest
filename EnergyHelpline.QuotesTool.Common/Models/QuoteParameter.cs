using System;

namespace EnergyHelpline.QuotesTool.Common.Models
{
    public class QuoteParameter
    {
        public double GasUsage { get; set; }
        public double ElectricityUsage { get; set; }
        public DateTime TimeOfQuote { get; set; }
        public User QuoteUser { get; set; }
    }
}
