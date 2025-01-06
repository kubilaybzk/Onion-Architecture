using MediatR;
using OnionArch.Application.CQRS_Globals.Pagination;


namespace OnionArch.Application.Features.Queries.DiscountCoupon.GetAllDiscountCoupons
{
    public class GetAllDiscountCouponsQueryRequest : IRequest<GetAllDiscountCouponsQueryResponse>
    {
        public GlobalPaginationRequest Pagination { get; set; }
    }
} 