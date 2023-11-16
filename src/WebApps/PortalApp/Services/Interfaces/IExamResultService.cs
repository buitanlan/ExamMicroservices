using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;

namespace PortalApp.Services.Interfaces;

public interface IExamResultService
{
    Task<ApiResult<ExamResultDto>> GetExamResultByIdAsync(string id);
    Task<ApiResult<ExamResultDto>> NextQuestionAsync(NextQuestionRequest request);
    Task<ApiResult<bool>> SkipExamAsync(SkipExamRequest request);
    Task<ApiResult<ExamResultDto>> FinishExamAsync(FinishExamRequest request);
    Task<ApiResult<ExamResultDto>> StartExamAsync(StartExamRequest request);
    Task<ApiResult<PagedList<ExamResultDto>>> GetExamResultsByUserIdPagingAsync(PagingParameters request);
}
