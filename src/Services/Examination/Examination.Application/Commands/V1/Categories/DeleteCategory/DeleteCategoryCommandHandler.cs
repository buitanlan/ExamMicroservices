using Examination.Domain.AggregateModels.CategoryAggregate;
using Examination.Shared.SeedWork;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler(ICategoryRepository categoryRepository) : IRequestHandler<DeleteCategoryCommand, ApiResult<bool>>
{
    public async Task<ApiResult<bool>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await categoryRepository.GetCategoriesByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return new ApiErrorResult<bool>("Item is not found {request.Id}");
        }

        try
        {
            await categoryRepository.DeleteAsync(request.Id);
            return new ApiSuccessResult<bool>(true, "Delete successful");;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex.Message);
            throw;
        }
    }
}