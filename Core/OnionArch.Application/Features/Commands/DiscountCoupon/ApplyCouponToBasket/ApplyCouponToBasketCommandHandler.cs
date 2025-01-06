using MediatR;
using OnionArch.Application.Abstractions.DiscountServices;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.ApplyCouponToBasket
{
    public class ApplyCouponToBasketCommandHandler : IRequestHandler<ApplyCouponToBasketCommandRequest, ApplyCouponToBasketCommandResponse>
    {
        private readonly IDiscountCouponService _discountCouponService;

        public ApplyCouponToBasketCommandHandler(IDiscountCouponService discountCouponService)
        {
            _discountCouponService = discountCouponService;
        }

        public async Task<ApplyCouponToBasketCommandResponse> Handle(ApplyCouponToBasketCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                bool applied = await _discountCouponService.ApplyCouponToBasketAsync(request.CouponCode);

                if (!applied)
                {
                    return new ApplyCouponToBasketCommandResponse
                    {
                        HassError = true,
                        ErrorMessage = "Kupon uygulanamadı",
                        StatusCode = System.Net.HttpStatusCode.BadRequest
                    };
                }

                return new ApplyCouponToBasketCommandResponse
                {
                    HassError = false,
                    Message = "Kupon başarıyla uygulandı",
                    StatusCode = System.Net.HttpStatusCode.OK
                };
            }
            catch (Exception ex)
            {
                return new ApplyCouponToBasketCommandResponse
                {
                    HassError = true,
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
    }
} 