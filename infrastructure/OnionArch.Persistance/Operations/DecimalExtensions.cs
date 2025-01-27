using System;
using System.Globalization;

namespace OnionArch.Persistance.Operations
{
    public static class DecimalExtensions
    {
        private static readonly CultureInfo TurkishCulture = new CultureInfo("tr-TR");

        public static decimal ToTurkishLira(this string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;

            // Scientific notation check
            if (value.Contains('E')) return 0;

            // Try parse with invariant culture
            if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
            {
                return result;
            }

            return 0;
        }
    }
}