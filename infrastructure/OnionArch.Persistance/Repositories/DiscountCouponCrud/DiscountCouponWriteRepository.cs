using OnionArch.Application.Repositories.DiscountCouponCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositories;

namespace OnionArch.Persistance.Repositories.DiscountCouponCrud
{
    public class DiscountCouponWriteRepository : WriteRepository<DiscountCoupon>, IDiscountCouponWriteRepository
    {
        public DiscountCouponWriteRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}