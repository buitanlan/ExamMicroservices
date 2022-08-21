using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler: IRequestHandler<UpdateCategoryCommand, ApiResult<bool>>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

    }

    public async Task<ApiResult<bool>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>($"Item is not found {request.Id}");
        }

        itemToUpdate.Name = request.Name;
        itemToUpdate.UrlPath = request.UrlPath;
      
        await _categoryRepository.UpdateAsync(itemToUpdate);
       

        return new ApiSuccessResult<bool>(true, "Update successful");
    }
}