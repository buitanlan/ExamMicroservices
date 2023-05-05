using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Commands.V1.Questions.DeleteQuestion;

public class DeleteQuestionCommand(string id) : IRequest<ApiResult<bool>>
{
    public string Id { get; set; } = id;
}