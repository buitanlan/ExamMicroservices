using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;

namespace Examination.Application.Queries.V1.ExamResults.GetExamResultsPaging;

public class GetExamResultsPagingQuery: IRequest<ApiResult<PagedList<ExamResultDto>>>
{
    public string Keyword { get; set; }
    public int PageIndex { get; set; }
    public int PageSize { get; set; }
}
