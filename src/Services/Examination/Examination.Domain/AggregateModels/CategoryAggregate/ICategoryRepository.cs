using Examination.Domain.SeedWord;
using Examination.Shared.SeedWork;

namespace Examination.Domain.AggregateModels.CategoryAggregate;

public interface ICategoryRepository: IRepositoryBase<Category>
{
    Task<List<Category>> GetAllCategoriesAsync();
    Task<PagedList<Category>> GetCategoriesPagingAsync(string searchKeyword, int pageIndex, int pageSize);

    Task<Category> GetCategoriesByIdAsync(string id);

    Task<Category> GetCategoriesByNameAsync(string name);
}