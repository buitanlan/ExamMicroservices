﻿using System.Net.Http.Json;
using AdminApp.Services.Interfaces;
using Examination.Shared.Questions;
using Examination.Shared.SeedWork;
using Microsoft.AspNetCore.WebUtilities;

namespace AdminApp.Services;

public class QuestionService(HttpClient httpClient) : IQuestionService
{
    public async Task<bool> CreateAsync(CreateQuestionRequest request)
    {
        var result = await httpClient.PostAsJsonAsync("/api/v1/Questions", request);
        return result.IsSuccessStatusCode;
    }

    public async Task<bool> DeleteAsync(string id)
    {
        var result = await httpClient.DeleteAsync($"/api/v1/Questions/{id}");
        return result.IsSuccessStatusCode;
    }

    public async Task<ApiResult<QuestionDto>> GetQuestionByIdAsync(string id)
    {
        var result = await httpClient.GetFromJsonAsync<ApiResult<QuestionDto>>($"/api/v1/Questions/{id}");
        return result;
    }

    public async Task<ApiResult<PagedList<QuestionDto>>> GetQuestionsPagingAsync(QuestionSearch searchInput)        {
        var queryStringParam = new Dictionary<string, string>
        {
            ["pageIndex"] = searchInput.PageNumber.ToString(),
            ["pageSize"] = searchInput.PageSize.ToString()
        };

        if (!string.IsNullOrEmpty(searchInput.Name))
            queryStringParam.Add("searchKeyword", searchInput.Name);

        if(!string.IsNullOrEmpty(searchInput.CategoryId))
            queryStringParam.Add("categoryId", searchInput.CategoryId);

        var url = QueryHelpers.AddQueryString("/api/v1/Questions/paging", queryStringParam);

        var result = await httpClient.GetFromJsonAsync<ApiResult<PagedList<QuestionDto>>>(url);
        return result;
    }

    public async Task<bool> UpdateAsync(UpdateQuestionRequest request)
    {
        var result = await httpClient.PutAsJsonAsync($"/api/v1/Questions", request);
        return result.IsSuccessStatusCode;
    }
}