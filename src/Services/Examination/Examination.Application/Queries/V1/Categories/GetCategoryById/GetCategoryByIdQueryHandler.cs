using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.Categories;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace Examination.Application.Queries.V1.Categories.GetCategoryById;

public class GetCategoryByIdQueryHandler(
    ICategoryRepository categoryRepository,
    IMapper mapper,
    IClientSessionHandle clientSessionHandle)
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{
    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetCategoryByIdQueryHandler");

        var result = await categoryRepository.GetCategoriesByIdAsync(request.Id);
        var item = mapper.Map<CategoryDto>(result);

        Log.Information("END: GetCategoryByIdQueryHandler");

        return item;
    }
}
