using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Queries.V1.ExamResults;

public class GetExamResultByIdQuery(string id) : IRequest<ApiResult<ExamResultDto>>
{
    public string Id { set; get; } = id;
}
