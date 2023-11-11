using Examination.Shared.Exams;
using Examination.Shared.SeedWork;

namespace PortalApp.Services.Interfaces;

public interface IExamService
{
    Task<ApiResult<PagedList<ExamDto>>> GetExamsPagingAsync(ExamSearch search);
    Task<ApiResult<ExamDto>> GetExamByIdAsync(string id);
}
