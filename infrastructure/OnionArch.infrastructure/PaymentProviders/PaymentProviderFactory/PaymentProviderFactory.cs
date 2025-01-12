using Microsoft.Extensions.Configuration;
using OnionArch.Application.Abstractions.PaymentServices;
using OnionArch.infrastructure.PaymentProviders.Iyzico;
using OnionArch.infrastructure.PaymentProviders.Sipay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.infrastructure.PaymentProviders.PaymentProviderFactory
{
    public class PaymentProviderFactory : IPaymentFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceProvider _serviceProvider;

        public PaymentProviderFactory(IConfiguration configuration, IServiceProvider serviceProvider)
        {
            _configuration = configuration;
            _serviceProvider = serviceProvider;
        }

        public IPaymentProvider CreateProvider(string providerName)
        {
            return providerName.ToLower() switch
            {
                "iyzico" => new IyzicoPaymentProvider(_configuration),
                "sipay" => new SipayPaymentProvider(_configuration),
                _ => throw new ArgumentException($"Unknown payment provider: {providerName}")
            };
        }
    }
}
