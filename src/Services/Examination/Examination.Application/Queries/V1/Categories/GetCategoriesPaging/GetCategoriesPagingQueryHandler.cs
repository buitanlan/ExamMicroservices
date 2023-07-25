using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.Categories;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace Examination.Application.Queries.V1.Categories.GetCategoriesPaging;

public class GetCategoriesPagingQueryHandler(ICategoryRepository categoryRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle)
    : IRequestHandler<GetCategoriesPagingQuery, ApiResult<PagedList<CategoryDto>>>
{

    private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    private readonly IClientSessionHandle _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));

    public async Task<ApiResult<PagedList<CategoryDto>>> Handle(GetCategoriesPagingQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetHomeExamListQueryHandler");

        var result = await _categoryRepository.GetCategoriesPagingAsync(request.SearchKeyword, request.PageIndex, request.PageSize);
        var items = mapper.Map<List<CategoryDto>>(result.Items);
        var pagedResult = new PagedList<CategoryDto>(items, result.MetaData.TotalCount, request.PageIndex, request.PageSize);
        return new ApiSuccessResult<PagedList<CategoryDto>>(pagedResult);
    }
}
