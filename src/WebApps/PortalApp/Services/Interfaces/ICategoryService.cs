using Examination.Shared.Categories;
using Examination.Shared.SeedWork;

namespace PortalApp.Services.Interfaces;

public interface ICategoryService
{
    Task<ApiResult<List<CategoryDto>>> GetAllCategoriesAsync();
}
