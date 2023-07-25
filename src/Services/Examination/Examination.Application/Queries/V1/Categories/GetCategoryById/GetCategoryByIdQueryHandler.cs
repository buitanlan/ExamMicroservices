using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.Categories;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace Examination.Application.Queries.V1.Categories.GetCategoryById;

public class GetCategoryByIdQueryHandler(ICategoryRepository categoryRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle)
    : IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{

    private readonly ICategoryRepository _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
    private readonly IClientSessionHandle _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetCategoryByIdQueryHandler");

        var result = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
        var item = mapper.Map<CategoryDto>(result);

        Log.Information("END: GetCategoryByIdQueryHandler");

        return item;
    }
}