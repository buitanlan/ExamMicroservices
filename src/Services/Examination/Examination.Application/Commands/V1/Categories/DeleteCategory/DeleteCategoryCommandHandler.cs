using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, ApiResult<bool>>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ApiResult<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>("Item is not found {request.Id}");
        }

        try
        {
            await _categoryRepository.DeleteAsync(request.Id);
            return new ApiSuccessResult<bool>(true, "Delete successful");;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex.Message);
            throw;
        }
    }
}