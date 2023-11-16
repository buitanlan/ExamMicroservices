using Examination.Shared.Categories;
using Examination.Shared.SeedWork;
using PortalApp.Services.Interfaces;

namespace PortalApp.Services;

public class CategoryService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    : BaseService(httpClientFactory, httpContextAccessor), ICategoryService
{
    public  Task<ApiResult<List<CategoryDto>>> GetAllCategoriesAsync()
        => GetAsync<List<CategoryDto>>("/api/v1/categories");
}
