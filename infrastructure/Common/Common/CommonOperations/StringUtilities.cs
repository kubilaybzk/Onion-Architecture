using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Common.CommonOperations
{
     
        public static class StringUtilities
        {
            public static string GenerateSlug(string phrase)
            {
                // Türkçe karakterleri çıkar
                string str = RemoveTurkishCharacters(phrase).ToLower();
                // Geçersiz karakterleri temizle
                str = Regex.Replace(str, @"[^a-z0-9\s-]", "");
                // Birden fazla boşluğu tek boşluğa dönüştür
                str = Regex.Replace(str, @"\s+", " ").Trim();
                // Boşlukları tireye dönüştür
                str = Regex.Replace(str, @"\s", "-");
                return str;
            }

            public static string RemoveTurkishCharacters(string input)
            {
                // Türkçe karakterleri çevirme
                input = input.Replace("ı", "i").Replace("İ", "I")
                             .Replace("ş", "s").Replace("Ş", "S")
                             .Replace("ğ", "g").Replace("Ğ", "G")
                             .Replace("ç", "c").Replace("Ç", "C")
                             .Replace("ö", "o").Replace("Ö", "O")
                             .Replace("ü", "u").Replace("Ü", "U");
                return input;
            }
        }
    
}
