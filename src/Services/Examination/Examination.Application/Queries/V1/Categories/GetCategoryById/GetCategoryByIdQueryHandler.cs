using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Dtos.Categories;
using MediatR;
using MongoDB.Driver;
using Serilog;

namespace Examination.Application.Queries.V1.Categories.GetCategoryById;

public class GetCategoryByIdQueryHandler: IRequestHandler<GetCategoryByIdQuery, CategoryDto>
{

    private readonly ICategoryRepository _categoryRepository;
    private readonly IClientSessionHandle _clientSessionHandle;
    private readonly IMapper _mapper;

    public GetCategoryByIdQueryHandler(
        ICategoryRepository categoryRepository,
        IMapper mapper,
        IClientSessionHandle clientSessionHandle
    )
    {
        _categoryRepository = categoryRepository ?? throw new ArgumentNullException(nameof(categoryRepository));
        _clientSessionHandle = clientSessionHandle ?? throw new ArgumentNullException(nameof(_clientSessionHandle));
        _mapper = mapper;

    }

    public async Task<CategoryDto> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        Log.Information("BEGIN: GetCategoryByIdQueryHandler");

        var result = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
        var item = _mapper.Map<CategoryDto>(result);

        Log.Information("END: GetCategoryByIdQueryHandler");

        return item;
    }
}