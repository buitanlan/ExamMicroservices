using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Queries.V1.Questions.GetQuestionById;

public class GetQuestionByIdQuery(string id) : IRequest<ApiResult<QuestionDto>>
{
    public string Id { set; get; } = id;
}