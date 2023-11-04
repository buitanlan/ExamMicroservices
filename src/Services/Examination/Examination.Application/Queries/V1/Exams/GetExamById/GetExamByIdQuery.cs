using Examination.Shared.Exams;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Queries.V1.Exams.GetExamById;

public class GetExamByIdQuery(string id) : IRequest<ApiResult<ExamDto>>
{
    public string Id { set; get; } = id;
}
