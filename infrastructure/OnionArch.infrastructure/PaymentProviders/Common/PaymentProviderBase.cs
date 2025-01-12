using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.infrastructure.PaymentProviders.Common
{
    public abstract class PaymentProviderBase
    {
        protected readonly IConfiguration _configuration;

        public PaymentProviderBase(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected string GetMaskedCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber)) return string.Empty;
            return $"{"*".PadLeft(cardNumber.Length - 4, '*')}{cardNumber.Substring(cardNumber.Length - 4)}";
        }
    }
}
