using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.DiscountCouponCrud;

namespace OnionArch.Application.Features.Queries.DiscountCoupon.GetAllDiscountCoupons
{
    public class GetAllDiscountCouponsQueryHandler : IRequestHandler<GetAllDiscountCouponsQueryRequest, GetAllDiscountCouponsQueryResponse>
    {
        private readonly IDiscountCouponReadRepository _discountCouponReadRepository;

        public GetAllDiscountCouponsQueryHandler(IDiscountCouponReadRepository discountCouponReadRepository)
        {
            _discountCouponReadRepository = discountCouponReadRepository;
        }

        public async Task<GetAllDiscountCouponsQueryResponse> Handle(GetAllDiscountCouponsQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _discountCouponReadRepository.GetAll();
                
                var totalCount = await query.CountAsync();

                var coupons = await query
                    .Skip(request.Pagination.Page * request.Pagination.Size)
                    .Take(request.Pagination.Size)
                    .Select(c => new DiscountCouponDto
                    {
                        Id = c.ID,
                        Code = c.Code,
                        Description = c.Description,
                        DiscountAmount = c.DiscountAmount,
                        MinimumCartAmount = c.MinimumCartAmount,
                        IsPercentage = c.IsPercentage,
                        ValidFrom = c.ValidFrom,
                        ValidUntil = c.ValidUntil,
                        IsPersonal = c.IsPersonal,
                        UserId = c.UserId,
                        MaxUsageCount = c.MaxUsageCount,
                        UsedCount = c.UsedCount,
                        IsActive = c.IsActive
                    })
                    .ToListAsync(cancellationToken);

                return new GetAllDiscountCouponsQueryResponse
                {
                    HassError = false,
                    StatusCode = System.Net.HttpStatusCode.OK,
                    TotalCount = totalCount,
                    Coupons = coupons
                };
            }
            catch (Exception ex)
            {
                return new GetAllDiscountCouponsQueryResponse
                {
                    HassError = true,
                    ErrorMessage = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.InternalServerError
                };
            }
        }
    }
} 