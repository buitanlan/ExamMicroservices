using Examination.Shared.Categories;
using MediatR;

namespace Examination.Application.Queries.V1.Categories.GetCategoryById;

public class GetCategoryByIdQuery(string id) : IRequest<CategoryDto>
{
    public string Id { set; get; } = id;
}