using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Application.Abstractions.PaymentServices
{
    public interface IPaymentFactory
    {
        IPaymentProvider CreateProvider(string providerName);
    }
}
