using AutoMapper;
using Examination.Domain.AggregateModels.ExamResultAggregate;
using Examination.Shared.ExamResults;
using Examination.Shared.SeedWork;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Examination.Application.Queries.V1.ExamResults.GetExamResultsPaging;

public class GetExamResultsByUserIdPagingQueryHandler(
        IExamResultRepository examResultRepository,
        IMapper mapper,
        ILogger<GetExamResultsByUserIdPagingQueryHandler> logger)
    : IRequestHandler<GetExamResultsPagingQuery, ApiResult<PagedList<ExamResultDto>>>
{
    public async Task<ApiResult<PagedList<ExamResultDto>>> Handle(GetExamResultsPagingQuery request, CancellationToken cancellationToken)
    {
        logger.LogInformation("BEGIN: GetHomeExamListQueryHandler");

        var result = await examResultRepository.GetExamResultsPagingAsync(
            request.Keyword,
            request.PageIndex,
            request.PageSize);

        var items = mapper.Map<List<ExamResultDto>>(result.Items);

        logger.LogInformation("END: GetHomeExamListQueryHandler");
        var pagedItems = new PagedList<ExamResultDto>(items, result.MetaData.TotalCount, request.PageIndex, request.PageSize);

        return new ApiSuccessResult<PagedList<ExamResultDto>>(200, pagedItems);
    }
}
