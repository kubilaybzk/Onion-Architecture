using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.DiscountCouponCrud;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.CreateDiscountCoupon
{
    public class CreateDiscountCouponCommandHandler : IRequestHandler<CreateDiscountCouponCommandRequest, CreateDiscountCouponCommandResponse>
    {
        private readonly IDiscountCouponWriteRepository _discountCouponWriteRepository;
        private readonly IDiscountCouponReadRepository _discountCouponReadRepository;

        public CreateDiscountCouponCommandHandler(
            IDiscountCouponWriteRepository discountCouponWriteRepository,
            IDiscountCouponReadRepository discountCouponReadRepository)
        {
            _discountCouponWriteRepository = discountCouponWriteRepository;
            _discountCouponReadRepository = discountCouponReadRepository;
        }

        public async Task<CreateDiscountCouponCommandResponse> Handle(CreateDiscountCouponCommandRequest request, CancellationToken cancellationToken)
        {
            var existingCoupon = await _discountCouponReadRepository
                .GetWhere(x => x.Code == request.Code)
                .FirstOrDefaultAsync();

            if (existingCoupon != null)
            {
                return new CreateDiscountCouponCommandResponse
                {
                    HassError = true,
                    ErrorMessage = "Bu kupon kodu zaten kullanımda",
                    StatusCode = System.Net.HttpStatusCode.BadRequest,
                    isCreated = false
                };
            }

            var coupon = new Domain.Entities.DiscountCoupon
            {
                Code = request.Code,
                Description = request.Description,
                DiscountAmount = request.DiscountAmount,
                MinimumCartAmount = request.MinimumCartAmount,
                IsPercentage = request.IsPercentage,
                ValidFrom = request.ValidFrom,
                ValidUntil = request.ValidUntil,
                IsPersonal = request.IsPersonal,
                UserId = request.UserId,
                MaxUsageCount = request.MaxUsageCount,
                UsedCount = 0,
                IsActive = true
            };

            await _discountCouponWriteRepository.AddAsync(coupon);
            await _discountCouponWriteRepository.SaveAsync();

            return new CreateDiscountCouponCommandResponse
            {
                HassError = false,
                Message = "Kupon başarıyla oluşturuldu",
                StatusCode = System.Net.HttpStatusCode.Created,
                CouponId = coupon.ID,
                Code = coupon.Code,
                isCreated = true
            };
        }
    }
} 