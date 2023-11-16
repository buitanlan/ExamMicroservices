using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;

namespace AdminApp.Services.Interfaces;

public interface IExamResultService
{
    Task<ApiResult<PagedList<ExamResultDto>>> GetExamResultsPagingAsync(ExamResultSearch search);
    Task<ApiResult<ExamResultDto>> GetExamResultByIdAsync(string id);
}
