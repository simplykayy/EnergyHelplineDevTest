using System;

namespace EnergyHelpline.QuotesTool.Common
{
    public static class Extensions
    {
        public static int GetDaysInYear(this DateTime currentDate)
        {
            DateTime thisYear = new DateTime(currentDate.Year, 1, 1);
            DateTime nextYear = new DateTime(currentDate.Year + 1, 1, 1);

            return (nextYear - thisYear).Days;
        }

        public static double CalculateFinalUnitRate(this double? finalUnitRate, double usage, double noOfDays)
        {
            if (finalUnitRate.HasValue)
                return noOfDays * usage * finalUnitRate.Value;

            return default(double);
        }
    }
}
