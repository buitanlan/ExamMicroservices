using System.Net;
using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.Categories;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Queries.V1.Categories.GetAllCategories;

public class GetAllCategoriesQueryHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper,
    ILogger<GetAllCategoriesQueryHandler> logger)
    : IRequestHandler<GetAllCategoriesQuery, ApiResult<List<CategoryDto>>>
{
    public async Task<ApiResult<List<CategoryDto>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("BEGIN: GetHomeExamListQueryHandler");

        var result = await categoryRepository.GetAllCategoriesAsync();
        var items = mapper.Map<List<CategoryDto>>(result);

        logger.LogInformation("END: GetHomeExamListQueryHandler");
        return new ApiSuccessResult<List<CategoryDto>>(HttpStatusCode.OK, items);
    }
}
