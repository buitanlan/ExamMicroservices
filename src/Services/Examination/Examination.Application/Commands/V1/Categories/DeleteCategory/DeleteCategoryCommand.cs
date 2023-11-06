using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Commands.V1.Categories.DeleteCategory;

public class DeleteCategoryCommand(string id) : IRequest<ApiResult<bool>>
{
    public string Id { get; set; } = id;
}
