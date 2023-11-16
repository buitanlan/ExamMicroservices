using AutoMapper;
using Examination.Application.Extensions;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Queries.V1.ExamResults.GetExamResultsByUserIdPaging;

public class GetExamResultsByUserIdPagingQueryHandler(IExamResultRepository examResultRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor,
        ILogger<GetExamResultsByUserIdPagingQueryHandler> logger)
    : IRequestHandler<GetExamResultsByUserIdPagingQuery, ApiResult<PagedList<ExamResultDto>>>
{
    public async Task<ApiResult<PagedList<ExamResultDto>>> Handle(GetExamResultsByUserIdPagingQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("BEGIN: GetHomeExamListQueryHandler");

        var result = await examResultRepository.GetExamResultsByUserIdPagingAsync(
            httpContextAccessor.GetUserId(),
            request.PageIndex,
            request.PageSize);

        var items = mapper.Map<List<ExamResultDto>>(result.Items);

        logger.LogInformation("END: GetHomeExamListQueryHandler");
        var pagedItems = new PagedList<ExamResultDto>(items, result.MetaData.TotalCount, request.PageIndex, request.PageSize);

        return new ApiSuccessResult<PagedList<ExamResultDto>>(200, pagedItems);
    }
}
