using System;
using System.Collections.Generic;

namespace EnergyHelpline.QuotesTool.Common.Models
{
    public class EnergyPlan
    {
        public string TariffName { get; set; }
        public double GasInitialUnitRate { get; set; }
        public double? GasFinalUnitRate { get; set; }
        public double ElectricityInitialUnitRate { get; set; }
        public double? ElectricityFinalUnitRate { get; set; }
        public DateTime? InitialRateExpirationDate { get; set; }

        public static IReadOnlyCollection<EnergyPlan> InitialisePlans()
        {
            IReadOnlyCollection<EnergyPlan> plans = new List<EnergyPlan>()
            {
                new EnergyPlan { TariffName = "Energy Saver", GasInitialUnitRate = 0.25, GasFinalUnitRate = 0.50, ElectricityInitialUnitRate = 0.30, ElectricityFinalUnitRate = 0.60, InitialRateExpirationDate = new DateTime(2016, 6, 1) },
                new EnergyPlan { TariffName = "Discount Energy", GasInitialUnitRate = 0.20, GasFinalUnitRate = 0.75, ElectricityInitialUnitRate = 0.20, ElectricityFinalUnitRate = 0.90, InitialRateExpirationDate = new DateTime(2016, 9, 15) },
                new EnergyPlan { TariffName = "Standard", GasInitialUnitRate = 0.65, GasFinalUnitRate = null, ElectricityInitialUnitRate = 0.80, ElectricityFinalUnitRate = null, InitialRateExpirationDate = null },
                new EnergyPlan { TariffName = "Save Online", GasInitialUnitRate = 0.25, GasFinalUnitRate = 0.60, ElectricityInitialUnitRate = 0.10, ElectricityFinalUnitRate = 1.00, InitialRateExpirationDate = new DateTime(2017, 1, 1) }
            };

            return plans;
        }
    }
}
