using OnionArch.Application.Repositories.PaymentTransactionCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnionArch.Persistance.Repositorys.PaymentTransactionCrud
{
    public class PaymentTransactionWriteRepository : WriteRepository<PaymentTransaction>, IPaymentTransactionWriteRepository
    {
        public PaymentTransactionWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}
