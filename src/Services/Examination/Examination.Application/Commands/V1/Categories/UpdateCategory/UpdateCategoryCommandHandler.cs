using Examination.Domain.AggregateModels.CategoryAggregate;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Categories.UpdateCategory;

public class UpdateCategoryCommandHandler: IRequestHandler<UpdateCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;

    public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;

    }

    public async Task<bool> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return false;
        }

        itemToUpdate.Name = request.Name;
        itemToUpdate.UrlPath = request.UrlPath;
        try
        {
            await _categoryRepository.UpdateAsync(itemToUpdate);
        }
        catch (Exception ex)
        {

            Log.Fatal(ex.Message);
            throw;
        }

        return true;
    }
}