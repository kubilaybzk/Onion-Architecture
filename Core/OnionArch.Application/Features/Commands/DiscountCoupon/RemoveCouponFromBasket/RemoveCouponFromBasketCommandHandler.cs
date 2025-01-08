using MediatR;
using OnionArch.Application.Abstractions.DiscountServices;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.RemoveCouponFromBasket
{
    public class RemoveCouponFromBasketCommandHandler : IRequestHandler<RemoveCouponFromBasketCommandRequest, RemoveCouponFromBasketCommandResponse>
    {
        private readonly IDiscountCouponService _discountCouponService;

        public RemoveCouponFromBasketCommandHandler(IDiscountCouponService discountCouponService)
        {
            _discountCouponService = discountCouponService;
        }

        public async Task<RemoveCouponFromBasketCommandResponse> Handle(RemoveCouponFromBasketCommandRequest request, CancellationToken cancellationToken)
        {
            try
            {
                bool removed = await _discountCouponService.RemoveCouponFromBasketAsync();

                if (!removed)
                {
                    return new RemoveCouponFromBasketCommandResponse
                    {
                        HassError = true,
                        ErrorMessage = "Sepette aktif kupon bulunmamaktadır",
                        StatusCode = System.Net.HttpStatusCode.BadRequest,
                        isDeleted = false

                    };
                }

                return new RemoveCouponFromBasketCommandResponse
                {
                    HassError = false,
                    Message = "Kupon başarıyla kaldırıldı",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    isDeleted = true
                };
            }
            catch (Exception ex)
            {
                return new RemoveCouponFromBasketCommandResponse
                {
                    HassError = true,
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError,
                    isDeleted = false
                };
            }
        }
    }
} 