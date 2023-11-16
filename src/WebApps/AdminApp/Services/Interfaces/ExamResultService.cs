using System.Net.Http.Json;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.WebUtilities;

namespace AdminApp.Services.Interfaces;

public class ExamResultService(HttpClient httpClient) : IExamResultService
{
    public async Task<ApiResult<ExamResultDto>> GetExamResultByIdAsync(string id)
    {
        var result = await httpClient.GetFromJsonAsync<ApiResult<ExamResultDto>>($"/api/v1/ExamResults/{id}");
        return result;
    }

    public async Task<ApiResult<PagedList<ExamResultDto>>> GetExamResultsPagingAsync(ExamResultSearch searchInput)
    {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageIndex"] = searchInput.PageNumber.ToString(),
            ["pageSize"] = searchInput.PageSize.ToString()
        };

        if (!string.IsNullOrEmpty(searchInput.Keyword))
            queryStringParam.Add("keyword", searchInput.Keyword);

        string url = QueryHelpers.AddQueryString("/api/v1/ExamResults/paging", queryStringParam);

        var result = await httpClient.GetFromJsonAsync<ApiResult<PagedList<ExamResultDto>>>(url);
        return result;
    }
}
