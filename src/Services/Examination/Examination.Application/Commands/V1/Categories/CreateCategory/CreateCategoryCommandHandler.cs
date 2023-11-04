using AutoMapper;
using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.Categories;
using Examination.Shared.SeedWork;
using MediatR;
using MongoDB.Bson;
using Serilog;

namespace Examination.Application.Commands.V1.Categories.CreateCategory;

public class CreateCategoryCommandHandler(ICategoryRepository categoryRepository, IMapper mapper)
    : IRequestHandler<CreateCategoryCommand, ApiResult<CategoryDto>>
{
    public async Task<ApiResult<CategoryDto>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemToAdd = await categoryRepository.GetCategoriesByNameAsync(request.Name);
        if (itemToAdd is not null)
        {
            Log.Fatal($"Item name existed: {request.Name}");
            return null;
        }
        itemToAdd = new Category(ObjectId.GenerateNewId().ToString(), request.Name, request.UrlPath);
       
        await categoryRepository.InsertAsync(itemToAdd);
        var result = mapper.Map<Category, CategoryDto>(itemToAdd);
        return new ApiSuccessResult<CategoryDto>(200, result);
    }
}
