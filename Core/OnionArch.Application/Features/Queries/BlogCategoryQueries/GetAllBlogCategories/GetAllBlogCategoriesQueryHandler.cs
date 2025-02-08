using MediatR;
using Microsoft.EntityFrameworkCore;
using OnionArch.Application.Repositories.BlogCategoryCruds;
using System.Net;

namespace OnionArch.Application.Features.Queries.BlogCategoryQueries.GetAllBlogCategories
{
    public class GetAllBlogCategoriesQueryHandler : IRequestHandler<GetAllBlogCategoriesQueryRequest, GetAllBlogCategoriesQueryResponse>
    {
        private readonly IBlogCategoryReadRepository _blogCategoryReadRepository;

        public GetAllBlogCategoriesQueryHandler(IBlogCategoryReadRepository blogCategoryReadRepository)
        {
            _blogCategoryReadRepository = blogCategoryReadRepository;
        }

        public async Task<GetAllBlogCategoriesQueryResponse> Handle(GetAllBlogCategoriesQueryRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = _blogCategoryReadRepository.Table.AsQueryable();

                if (request.IsActive.HasValue)
                    query = query.Where(c => c.IsActive == request.IsActive.Value);

                if (!string.IsNullOrEmpty(request.SearchByName))
                    query = query.Where(c => c.Name.ToLower().Contains(request.SearchByName.ToLower()));

                var totalCount = await query.CountAsync();

                var categories = await query
                    .OrderBy(c => c.DisplayOrder)
                    .Select(c => new
                    {
                        c.ID,
                        c.Name,
                        c.Description,
                        c.Slug,
                        c.IsActive,
                        c.DisplayOrder,
                        BlogCount = c.Blogs.Where(p=>p.IsPublished==true).Count()
                    })
                    .ToListAsync();

                return new GetAllBlogCategoriesQueryResponse
                {
                    Categories = categories,
                    TotalCount = totalCount,
                    HassError = false,
                    Message = "Blog kategorileri başarıyla getirildi",
                    StatusCode = HttpStatusCode.OK,
                    StatusCodeString = HttpStatusCode.OK.ToString()
                };
            }
            catch (Exception ex)
            {
                return new GetAllBlogCategoriesQueryResponse
                {
                    Categories = null,
                    ErrorMessage = ex.Message,
                    HassError = true,
                    Message = "Blog kategorileri getirilirken bir hata oluştu",
                    StatusCode = HttpStatusCode.InternalServerError,
                    StatusCodeString = HttpStatusCode.InternalServerError.ToString()
                };
            }
        }
    }
}