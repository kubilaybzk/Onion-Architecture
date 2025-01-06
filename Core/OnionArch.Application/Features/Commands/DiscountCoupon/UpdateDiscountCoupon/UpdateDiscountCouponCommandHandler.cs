using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.DiscountCouponCrud;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.UpdateDiscountCoupon
{
    public class UpdateDiscountCouponCommandHandler : IRequestHandler<UpdateDiscountCouponCommandRequest, UpdateDiscountCouponCommandResponse>
    {
        private readonly IDiscountCouponWriteRepository _discountCouponWriteRepository;
        private readonly IDiscountCouponReadRepository _discountCouponReadRepository;

        public UpdateDiscountCouponCommandHandler(
            IDiscountCouponWriteRepository discountCouponWriteRepository,
            IDiscountCouponReadRepository discountCouponReadRepository)
        {
            _discountCouponWriteRepository = discountCouponWriteRepository;
            _discountCouponReadRepository = discountCouponReadRepository;
        }

        public async Task<UpdateDiscountCouponCommandResponse> Handle(UpdateDiscountCouponCommandRequest request, CancellationToken cancellationToken)
        {
            var coupon = await _discountCouponReadRepository.GetByIdAsync(request.Id.ToString());
            if (coupon == null)
            {
                return new UpdateDiscountCouponCommandResponse
                {
                    HassError = true,
                    ErrorMessage = "Kupon bulunamadı",
                    StatusCode = System.Net.HttpStatusCode.NotFound
                };
            }

            coupon.Description = request.Description;
            coupon.DiscountAmount = request.DiscountAmount;
            coupon.MinimumCartAmount = request.MinimumCartAmount;
            coupon.ValidUntil = request.ValidUntil;
            coupon.MaxUsageCount = request.MaxUsageCount;
            coupon.IsActive = request.IsActive;

            _discountCouponWriteRepository.Update(coupon);
            await _discountCouponWriteRepository.SaveAsync();

            return new UpdateDiscountCouponCommandResponse
            {
                HassError = false,
                Message = "Kupon başarıyla güncellendi",
                StatusCode = System.Net.HttpStatusCode.OK,
                CouponId = coupon.ID,
                Code = coupon.Code,
                IsActive = coupon.IsActive
            };
        }
    }
} 