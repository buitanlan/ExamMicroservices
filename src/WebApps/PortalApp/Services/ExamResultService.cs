using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.WebUtilities;
using PortalApp.Services.Interfaces;

namespace PortalApp.Services;

public class ExamResultService(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    : BaseService(httpClientFactory, httpContextAccessor), IExamResultService
{
    public Task<ApiResult<ExamResultDto>> GetExamResultByIdAsync(string id)
        => GetAsync<ExamResultDto>($"/api/v1/ExamResults/{id}", true);

    public Task<ApiResult<ExamResultDto>> NextQuestionAsync(NextQuestionRequest request)
        => PostAsync<NextQuestionRequest, ExamResultDto>("/api/v1/ExamResults/next-question", request, true);

    public Task<ApiResult<bool>> SkipExamAsync(SkipExamRequest request)
        => PutAsync<SkipExamRequest, bool>("/api/v1/ExamResults/skip", request, true);

    public Task<ApiResult<ExamResultDto>> FinishExamAsync(FinishExamRequest request)
        => PostAsync<FinishExamRequest, ExamResultDto>("/api/v1/ExamResults/finish", request, true);

    public Task<ApiResult<ExamResultDto>> StartExamAsync(StartExamRequest request)
        => PostAsync<StartExamRequest, ExamResultDto>("/api/v1/ExamResults/start", request, true);


    public async Task<ApiResult<PagedList<ExamResultDto>>> GetExamResultsByUserIdPagingAsync(PagingParameters request)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageIndex"] = request.PageNumber.ToString(),
            ["pageSize"] = request.PageSize.ToString()
        };
        string url = QueryHelpers.AddQueryString("/api/v1/ExamResults/user", queryStringParam);

        var result = await GetAsync<PagedList<ExamResultDto>>(url, true);
        return result;
    }
}
