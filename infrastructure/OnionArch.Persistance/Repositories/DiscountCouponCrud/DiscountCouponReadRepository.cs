using OnionArch.Application.Repositories.DiscountCouponCrud;
using OnionArch.Domain.Entities;
using OnionArch.Persistance.Contexts;
using OnionArch.Persistance.Repositories;

namespace OnionArch.Persistance.Repositories.DiscountCouponCrud
{
    public class DiscountCouponReadRepository : ReadRepository<DiscountCoupon>, IDiscountCouponReadRepository
    {
        public DiscountCouponReadRepository(OnionArchDBContext context) : base(context)
        {
        }
    }
}