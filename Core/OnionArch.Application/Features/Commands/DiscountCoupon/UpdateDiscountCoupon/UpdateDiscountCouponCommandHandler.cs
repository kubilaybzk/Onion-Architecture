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
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    isUpdated = false
                };
            }

            coupon.Code = request.Code;
            coupon.Description = request.Description;
            coupon.DiscountAmount = request.DiscountAmount;
            coupon.MinimumCartAmount = request.MinimumCartAmount;
            coupon.IsPercentage = request.IsPercentage;
            coupon.ValidFrom = request.ValidFrom;
            coupon.ValidUntil = request.ValidUntil;
            coupon.IsPersonal = request.IsPersonal;
            coupon.UserId = request.UserId;
            coupon.MaxUsageCount = request.MaxUsageCount;
            coupon.IsActive = request.IsActive;
            


            _discountCouponWriteRepository.Update(coupon);
            await _discountCouponWriteRepository.SaveAsync();

            return new UpdateDiscountCouponCommandResponse
            {
                HassError = false,
                Message = "Kupon başarıyla güncellendi",
                StatusCode = System.Net.HttpStatusCode.OK,
                isUpdated = true
            };
        }
    }
} 