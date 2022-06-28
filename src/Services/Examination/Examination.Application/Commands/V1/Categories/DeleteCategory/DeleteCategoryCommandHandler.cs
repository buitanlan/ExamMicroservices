using Examination.Domain.AggregateModels.CategoryAggregate;
using MediatR;
using Serilog;

namespace Examination.Application.Commands.V1.Categories.DeleteCategory;

public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        var itemToUpdate = await _categoryRepository.GetCategoriesByIdAsync(request.Id);
        if (itemToUpdate == null)
        {
            Log.Fatal($"Item is not found {request.Id}");
            return false;
        }

        try
        {
            await _categoryRepository.DeleteAsync(request.Id);
            return true;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex.Message);
            throw;
        }
    }
}