using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.DiscountCouponCrud;

namespace OnionArch.Application.Features.Commands.DiscountCoupon.DeleteDiscountCoupon
{
    public class DeleteDiscountCouponCommandHandler : IRequestHandler<DeleteDiscountCouponCommandRequest, DeleteDiscountCouponCommandResponse>
    {
        private readonly IDiscountCouponWriteRepository _discountCouponWriteRepository;
        private readonly IDiscountCouponReadRepository _discountCouponReadRepository;

        public DeleteDiscountCouponCommandHandler(
            IDiscountCouponWriteRepository discountCouponWriteRepository,
            IDiscountCouponReadRepository discountCouponReadRepository)
        {
            _discountCouponWriteRepository = discountCouponWriteRepository;
            _discountCouponReadRepository = discountCouponReadRepository;
        }

        public async Task<DeleteDiscountCouponCommandResponse> Handle(DeleteDiscountCouponCommandRequest request, CancellationToken cancellationToken)
        {
            var coupon = await _discountCouponReadRepository.GetByIdAsync(request.Id.ToString());
            if (coupon == null)
            {
                return new DeleteDiscountCouponCommandResponse
                {
                    HassError = true,
                    ErrorMessage = "Kupon bulunamadı",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    isDeleted = false
                };
            }

            _discountCouponWriteRepository.Remove(coupon);
            await _discountCouponWriteRepository.SaveAsync();

            return new DeleteDiscountCouponCommandResponse
            {
                HassError = false,
                Message = "Kupon başarıyla silindi",
                StatusCode = System.Net.HttpStatusCode.OK,
                DeletedCouponId = coupon.ID,
                isDeleted = true
            };
        }
    }
} 