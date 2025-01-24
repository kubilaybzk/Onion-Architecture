using OnionArch.Application.Repositories.PaymentTransactionCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositories.PaymentTransactionCrud
{
    public class PaymentTransactionReadRepository : ReadRepository<PaymentTransaction>, IPaymentTransactionReadRepository
    {
        public PaymentTransactionReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
