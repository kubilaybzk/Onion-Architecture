using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Operations
{
    public static class DecimalExtensions
    {
        
            private static readonly CultureInfo TurkishCulture = new CultureInfo("tr-TR");

            public static decimal ToTurkishLira(this string value)
            {
                if (string.IsNullOrEmpty(value))
                    return 0;

                // Bilimsel notasyonu temizle
                if (value.Contains('E'))
                    return 0;

                // Eğer tam sayı gelirse (örn: "9")
                if (value.All(char.IsDigit))
                {
                    if (decimal.TryParse(value, out decimal intResult))
                    {
                        string turkishFormat = intResult.ToString("N2", TurkishCulture);
                        return decimal.Parse(turkishFormat, TurkishCulture);
                    }
                }

                // Ondalıklı sayı gelirse (örn: "9.50000000")
                if (decimal.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture, out decimal result))
                {
                    string turkishFormat = result.ToString("N2", TurkishCulture);
                    return decimal.Parse(turkishFormat, TurkishCulture);
                }

                return 0;
            }
        
    }
}
